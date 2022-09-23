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
        private const int RIGHT_MOUSE_BTN = 2;
        private const int LEFT_MOUSE_BTN = 1;

        private bool _ready = false;



        private Vector2 min, max;


        public override async void _Ready()
        {
            _ready = false;
            await GasStuff.WaitForAssignments();

            if (Debug.AssertNotNull(GasStuff.BlockTilemap))
                _ready = true;
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
                            RecordCommand(_inProgressCommand.Result);
                        }
                    }
                }
                else
                {
                    Brush.MouseReleased();
                }
                // if (mbEvent.Pressed)
                // {
                //     var cell = GasStuff.BlockTilemap.WorldToMap(GetGlobalMousePosition());
                //     if (GasStuff.BlockTilemap.IsCellEditable(cell))
                //     {
                //         if (mbEvent.ButtonIndex == LEFT_MOUSE_BTN)
                //         {
                //             BuildBlockOnCell(cell);
                //             try
                //             {
                //                 GasStuff.GasTilemap.ClearCells(GasStuff.BlockTilemap.GetGasCellsOnCell(cell));
                //             }
                //             catch (Exception e)
                //             {
                //                 Debug.Log($"Exception: {e}");
                //
                //             }
                //         }
                //         else if (mbEvent.ButtonIndex == RIGHT_MOUSE_BTN)
                //         {
                //             RemoveBlockOnCell(cell);
                //         }
                //     }
                //     else
                //     {
                //         Debug.Log($"Cursor is on cell: {cell} which is outside rect ");
                //     }
                // }

            }

            base._Input(@event);
        }

        private void RecordCommand(IBrushCommand result)
        {
            _undoStack.Push(result);
        }

        private IBrush Brush
        {
            get => _brush == null ?  (_brush = new BlockBrush()) : _brush;
        }
        private IBrush _brush;
        private Task<IBrushCommand> _inProgressCommand;
        private Stack<IBrushCommand> _undoStack = new Stack<IBrushCommand>();
        private Stack<IBrushCommand> _redoStack = new Stack<IBrushCommand>();

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.UndoCommand();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.UndoCommand();
                _undoStack.Push(command);
            }
        }

        private async void OnMouse(Vector2 vector2)
        {
            if (_brush == null)
            {
                _brush = new BlockBrush();
                
            }
        }
        

        private void BuildBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.BuildSolidBlock(cell))
            {
                this.GetTree().SetInputAsHandled();
            }
        }

        private void RemoveBlockOnCell(Vector2 cell)
        {
            if (GasStuff.BlockTilemap == null) return;
            if (GasStuff.BlockTilemap.RemoveSolidBlock(cell))
            {
                this.GetTree().SetInputAsHandled();
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