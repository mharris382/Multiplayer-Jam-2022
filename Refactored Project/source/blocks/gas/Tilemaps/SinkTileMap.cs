using Game.core;
using Godot;

namespace Game.Blocks.Gas.Tilemaps
{
    public class SinkTileMap : TileMap
    {
        [Export()]
        public int sinkID = 2;

        [Export()] public int sinkValue = 6;
    
        public override void _Ready()
        {
            Debug.AssertNotNull(TileSet);
            Debug.Log($"Sink tiles are {TileSet.TileGetName(sinkID)}");
            var cells = GetUsedCells();
            if(cells == null)
                return;
            var cnt = cells.Count;
            Debug.Log($"Found {cnt} sink cells");
            foreach (var cell in cells)
            {
                var v = (Vector2)cell;
                GasStuff.AddSink(v, sinkValue);
            }
        }
    
    
    
    }
}
