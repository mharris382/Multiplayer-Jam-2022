using Godot;
using System;
using System.Collections.Generic;
using Game.Fluid.Core;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace Game.Fluid.GodotAdapter
{
    public class FluidAdapter : Node, IFluidAdapter
    {
        private TileMap _blockTileMap;
        private TileMap _gasTileMap;
        private Timer _timer;

        private Dictionary<Cell, List<FluidSink>> _sinks = new Dictionary<Cell, List<FluidSink>>();
        private Dictionary<Cell, List<FluidSource>> _sources = new Dictionary<Cell, List<FluidSource>>();

        public void Setup(TileMap gasTileMap, TileMap blockTileMap, Timer timer)
        {
            Print("Setup called on Fluid Adapter");
            _gasTileMap = gasTileMap;
            _blockTileMap = blockTileMap;
            _timer = timer;
        }

        public void RegisterFluidSink(FluidSink sink)
        {
            var cell = sink.GetTilemapCell(_gasTileMap);
            GD.Print($"Registered Fluid Sink {sink.Name} at cell {cell}");
            
            if (_sinks.TryGetValue(cell, out var list))
            {
                if (list.Contains(sink))
                    return;
                list.Add(sink);
            }
            else
            {
                list = new List<FluidSink> { sink };
                _sinks.Add(cell, list);
            }
        }
        
        public void RegisterFluidSource(FluidSource source)
        {
            var cell = source.GetTilemapCell(_gasTileMap);
            GD.Print($"Registered Fluid Source: {source.Name} at cell {cell}");
            
            if (_sources.TryGetValue(cell, out var list))
            {
                if (list.Contains(source))
                    return;
                list.Add(source);
            }
            else
            {
                list = new List<FluidSource> { source };
                _sources.Add(cell, list);
            }
        }

        public override void _Ready()
        {
            Print("Started Fluid Adapter");
            _timer.Connect("timeout", this, "_Iteration");
            _timer.Start();
        }

        public void _Iteration()
        {
            Print("Iterate Fluid ");
        }

        private void Print(string msg, params object[] args)
        {
            #if GODOT
            //stuff
            GD.Print(msg, args);
            #else
            //not stuff
            #endif
            
        }

        public bool IsCellBlocked(Cell cell)
        {
            return _blockTileMap.GetCell(cell.x, cell.y) != -1;
        }

        public int GetGas(Cell cell)
        {
            return (int)_gasTileMap.Call("get_steam", cell.x, cell.y);
        }

        public void SetGas(Cell cell, int amount)
        {
            _gasTileMap.Call("set_steam", cell.x, cell.y, amount);
        }
    }

}