using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Blocks.Gas;
using Game.Blocks.Gas.AirCurrents;
using Game.core;
using Godot;

namespace Game.Blocks.Solids
{
    /// <summary>
    /// adding this script to a scene will allow editing the block tilemaps at runtime to experiment with how the gas
    /// system behaves when the block map is changed.
    ///
    /// <para>
    /// For the first iteration, blocks will not consider the gas that is on the cell when being placed.  Eventually
    /// this may change. 
    /// </para>
    /// </summary>
    public class RuntimeBlockEditor : Node2D
    {

        [Signal]
        delegate void OnGasHoverChanged(Vector2 gasCell);

        [Signal]
        delegate void OnBlockHoverChanged(Vector2 blockCell);

        [Signal]
        delegate void OnBlockBuilt(Vector2 blockCelll);

        [Signal]
        delegate void OnBlockRemoved(Vector2 blockCell);
        
        private const string ON_BLOCK_BUILT_SIGNAL = "OnBlockBuilt";
        private const string ON_BLOCK_REMOVE_SIGNAL = "OnBlockRemoved";
        
        private const int RIGHT_MOUSE_BTN = 2;
        private const int LEFT_MOUSE_BTN = 1;
        private const int MIDDLE_MOUSE_BTN = 3;
        private bool _ready = false;

        [Export()]
        public Vector2 gasSize = Vector2.One;

        [Export()]
        public int gasAmount = 3;
        
        
        private Vector2 min, max;

        private bool _dragging;

        private Vector2 _lastGasCell;
        private Vector2 _lastBlockCell;

        private const int GAS = 0;
        private const int BLOCK = 1;

        private Vector2[] _startDragCell = new Vector2[2];
        private Vector2[] _dragCells = new Vector2[2];

        public override async void _Ready()
        {
            _ready = false;
            await GasStuff.WaitForAssignments();

            if (Debug.AssertNotNull(GasStuff.BlockTilemap))
                _ready = true;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            var mp = GetGlobalMousePosition();
            //if (_dragging)
            //{
                var gasCell = GasStuff.GasTilemap.WorldToMap(mp);
                if (gasCell != _dragCells[GAS])
                {
                    EmitSignal("OnGasHoverChanged", GasStuff.GasTilemap.MapToWorld(gasCell));
                    _dragCells[GAS] = gasCell;
                }

                var blockOffset = GasStuff.BlockTilemap.CellSize / 2;
                var blockCell =  GasStuff.BlockTilemap.GetHoveringCell();
                if (blockCell != _dragCells[BLOCK])
                {
                        EmitSignal("OnBlockHoverChanged", GasStuff.BlockTilemap.GetHoveringCell());
                        _dragCells[BLOCK] = blockCell;
                }
            //}
        }

        public override async void _Input(InputEvent @event)
        {
            if (!_ready || GasStuff.BlockTilemap == null)
            {
                Debug.Log("Error maps not assigned yet");
                return;
            }

            if (@event is InputEventMouseButton mbEvent)
            {
                if (mbEvent.Pressed)
                {
                    HandleMouseButtonEvent(mbEvent);
                    _dragging = true;
                }
                else
                {
                    _dragging = false;
                }
            }

            base._Input(@event);

            #region [Playing with code version]

#if BRUSH
            if (@event is InputEventMouseButton mbEvent)
            {
                if (mbEvent.Pressed)
                {
                    if (_inProgressCommand != null && _inProgressCommand.Status == TaskStatus.Running)
                    {
                        return;
                    }
                    else
                    {
                        _inProgressCommand = Brush.MouseJustPressed(GetGlobalMousePosition());
                        await _inProgressCommand;
                        if (_inProgressCommand.Status == TaskStatus.RanToCompletion)
                        {
                            
                        }
                    }
                }
                else
                {
                    Brush.MouseReleased();
                }
            }
#endif

            #endregion
        }

        private void HandleMouseButtonEvent(InputEventMouseButton mbEvent)
        {
            var gasCell = GasStuff.BlockTilemap.WorldToMap(GetGlobalMousePosition());
            var blockCell = GasStuff.BlockTilemap.GetHoveringCell();
            if (GasStuff.BlockTilemap.IsCellEditable(blockCell))
            {
                if (mbEvent.ButtonIndex == LEFT_MOUSE_BTN)
                {
                    BuildBlockOnCell(blockCell);
                    
                }
                else if (mbEvent.ButtonIndex == RIGHT_MOUSE_BTN)
                {
                    RemoveBlockOnCell(blockCell);
                }
                else if (mbEvent.ButtonIndex == MIDDLE_MOUSE_BTN)
                {
                    AddGasToCell(gasCell);
                }
                else
                {
                    Debug.Log($"Cursor is on cell: {gasCell} which is outside rect ");
                }
            }
        }

        private void BuildBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.BuildSolidBlock(cell))
            {
                OnBlockCreation(cell);
                this.GetTree().SetInputAsHandled();
            }
        }

        private void RemoveBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.RemoveSolidBlock(cell))
            {
                OnBlockRemoval(cell);
                this.GetTree().SetInputAsHandled();
            }
        }

        private void OnBlockCreation(Vector2 cell)
        {
            try
            {
                GasStuff.GasTilemap.ClearCells(GasStuff.BlockTilemap.GetGasCellsOnCell(cell));
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
            EmitSignal(ON_BLOCK_BUILT_SIGNAL, cell);
        }

        private void OnBlockRemoval(Vector2 cell)
        {
            EmitSignal(ON_BLOCK_REMOVE_SIGNAL, cell);
        }


        private void AddGasToCell(Vector2 cell)
        {
            for (int i = 0; i < gasSize.x; i++)
            {
                for (int j = 0; j < gasSize.y; j++)
                {
                    var offset = new Vector2(i, j);
                    GasStuff.GasTilemap.ModifySteam(cell + offset, gasAmount);
                }
            }
        }
        
        
    }
}

#region [Playing with code version]

#if BRUSH

        private IBrush Brush
        {
            get => _brush == null ? (_brush = new BlockBrush()) : _brush;
        }

        private IBrush _brush;
        private Task<IBrushCommand> _inProgressCommand;


        private async void OnMouse(Vector2 vector2)
        {
            if (_brush == null)
            {
                _brush = new BlockBrush();
            }
        }


        
    }


    public interface IBrush
    {
        Task<IBrushCommand> MouseJustPressed(Vector2 cell);
        void MouseReleased();
    }


    public interface IBrushCommand
    {
        void UndoCommand();
        void RedoCommand();
    }

    public class BlockBrush : Node2D, IBrush
    {
        [Signal]
        delegate void OnBlockBuilt(Vector2 cell);

        [Signal]
        delegate void OnBlockRemoved(Vector2 cell);

        private struct BuildBlockCommand : IBrushCommand
        {
            public Vector2 blockCell;


            public void UndoCommand()
            {
                GasStuff.BlockTilemap.RemoveSolidBlock(blockCell);
            }

            public void RedoCommand()
            {
                GasStuff.BlockTilemap.BuildSolidBlock(blockCell);
                GasStuff.GasTilemap.ClearCells(GasStuff.BlockTilemap.GetGasCellsOnCell(blockCell));
            }
        }

        private void BuildBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.BuildSolidBlock(cell))
            {
                EmitSignal(nameof(OnBlockBuilt));
                this.GetTree().SetInputAsHandled();
            }
        }

        private void RemoveBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.RemoveSolidBlock(cell))
            {
                EmitSignal(nameof(OnBlockRemoved));
                this.GetTree().SetInputAsHandled();
            }
        }

        public async Task<IBrushCommand> MouseJustPressed(Vector2 mp)
        {
            await GasStuff.WaitForAssignments();
            var command = new BuildBlockCommand()
            {
                blockCell = GasStuff.BlockTilemap.WorldToMap(mp)
            };
            command.RedoCommand();
            return command;
        }


        public void MouseReleased()
        {
        }
    }
}
#endif
  #endregion