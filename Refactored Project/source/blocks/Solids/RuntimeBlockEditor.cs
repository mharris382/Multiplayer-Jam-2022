using Game.Blocks.Gas;
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
                            GasStuff.GasTilemap.ClearCells(_solidBlockTilemap.GetGasCellsInBlockCell(cell));
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
                Debug.Log($"Building On Cell: {cell}");
                this.GetTree().SetInputAsHandled();
            }
        }

        private void RemoveBlockOnCell(Vector2 cell)
        {
            if (_solidBlockTilemap.RemoveSolidBlock(cell))
            {
                Debug.Log($"Destroying On Cell: {cell}");
                this.GetTree().SetInputAsHandled();
            }
        }

        
    }
}