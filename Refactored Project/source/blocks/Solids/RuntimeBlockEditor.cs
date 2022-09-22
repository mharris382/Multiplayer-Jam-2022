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

        [Signal]
        delegate void OnBlockBuilt(Vector2 cell);
        [Signal]
        delegate void OnBlockRemoved(Vector2 cell);
        
        private Vector2 min, max;

        private SolidBlockTilemap _solidBlockTilemap;
        
        public override async void _Ready()
        {
            _ready = false;
            await  GasStuff.WaitForAssignments();
            _solidBlockTilemap = GasStuff.BlockTilemap;

            if(Debug.AssertNotNull(_solidBlockTilemap))
                _ready = true;
        }

        public override void _Input(InputEvent @event)
        {
            if (!_ready)
            {
                Debug.Log("Error maps not assigned yet");
                return;
            }
            
            if (@event is InputEventMouseButton mbEvent)
            {
                if (mbEvent.Pressed)
                {
                    var cell = _solidBlockTilemap.WorldToMap(GetGlobalMousePosition());
                    if (_solidBlockTilemap.IsCellEditable(cell))
                    {
                        if (mbEvent.ButtonIndex == LEFT_MOUSE_BTN)
                        {
                            BuildBlockOnCell(cell);
                            GasStuff.GasTilemap.ClearCells(_solidBlockTilemap.GetGasCellsOnCell(cell));
                        }
                        else if (mbEvent.ButtonIndex == RIGHT_MOUSE_BTN)
                        {
                            RemoveBlockOnCell(cell);
                        }
                    }
                    else
                    {
                        Debug.Log($"Cursor is on cell: {cell} which is outside rect ");
                    }
                }
                
            }
            base._Input(@event);
        }
        
        

        private void BuildBlockOnCell(Vector2 cell)
        {
            if (_solidBlockTilemap.BuildSolidBlock(cell))
            {
                EmitSignal(nameof(OnBlockBuilt));
                this.GetTree().SetInputAsHandled();
            }
        }

        private void RemoveBlockOnCell(Vector2 cell)
        {
            if (_solidBlockTilemap.RemoveSolidBlock(cell))
            {
                EmitSignal(nameof(OnBlockRemoved));
                this.GetTree().SetInputAsHandled();
            }
        }
    }
}