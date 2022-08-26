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

        private Dictionary<Vector2Int, List<FluidSink>> _sinks = new Dictionary<Vector2Int, List<FluidSink>>();
        private Dictionary<Vector2Int, List<FluidSource>> _sources = new Dictionary<Vector2Int, List<FluidSource>>();

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

        public bool IsCellBlocked(Vector2Int vector2Int)
        {
            return _blockTileMap.GetCell(vector2Int.x, vector2Int.y) != -1;
        }

        public int GetGas(Vector2Int vector2Int)
        {
            return (int)_gasTileMap.Call("get_steam", vector2Int.x, vector2Int.y);
        }

        public void SetGas(Vector2Int vector2Int, int amount)
        {
            _gasTileMap.Call("set_steam", vector2Int.x, vector2Int.y, amount);
        }
    }

}