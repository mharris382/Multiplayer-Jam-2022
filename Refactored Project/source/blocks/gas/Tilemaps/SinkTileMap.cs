using System;
using System.Collections.Generic;
using System.Linq;
using Game.Blocks.Fluid;
using Game.core;
using Godot;

namespace Game.Blocks.Gas.Tilemaps
{
    public class SinkTileMap : TileMap, FluidIO.ISinks
    {
        [Export()]
        public int sinkID = 2;

        [Export()] public int sinkValue = 6;


        private List<CellSink> _sinks;
        public override void _Ready()
        {
            _sinks = new List<CellSink>();
            foreach (var usedCell in GetUsedCells())
            {
                var vec = (Vector2)usedCell;
                _sinks.Add(new CellSink(vec, (byte)(GetCellv(vec) + 1)));
            }
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

        
        public IEnumerable<CellSink> GetActiveSinks(Predicate<Vector2Int> condition)
        {
            return (from c in _sinks 
                where condition(c.GasPosition) 
                select c);
        }
    }
}
