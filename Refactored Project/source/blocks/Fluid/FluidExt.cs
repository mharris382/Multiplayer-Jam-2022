using System.Collections.Generic;
using Game.Blocks.Gas.Tilemaps;
using Godot;

namespace Game.Blocks.Fluid
{
    public static class FluidExt
    {
        internal static IEnumerable<CellSource> GetSourceCells(this GasSourceTilemap sourceTilemap)
        {
            for (int i = 0; i < 16; i++)
            {
                var cells = sourceTilemap.GetUsedCellsById(i);
                foreach (var cell in cells)
                {
                    var v = (Vector2)cell;
                    yield return new CellSource(v, (byte)i);
                }
            }
        }
        
        internal static IEnumerable<CellSink> GetSinkCells(this SinkTileMap sinkTileMap)
        {
            for (int i = 0; i < 16; i++)
            {
                var cells = sinkTileMap.GetUsedCellsById(i);
                foreach (var cell in cells)
                {
                    var v = (Vector2)cell;
                    yield return new CellSink(v, (byte)i);
                }
            }
        }
    }
}