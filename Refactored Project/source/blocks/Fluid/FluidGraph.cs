using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.core;
using Godot;

namespace Game.Blocks.Fluid
{
    public class FluidGraphBase<T> : Graph<T> where T : struct
    {
        public FluidGraphBase(GraphType type) : base(type)
        {
        }
    }

    public class GasGraph : FluidGraphBase<Vector2Int>
    {
        protected GasGraph() : base(GraphType.DIRECTED_WEIGHTED)
        {
        }
    }

    /// <summary> maps data type T to a gas graph key </summary>
    /// <typeparam name="T"></typeparam>
    public class GasGraph<T> : GasGraph where T : struct
    {
        private CellDataDictionary<T> _dataDictionary;

        protected GasGraph()
        {
            _dataDictionary = new CellDataDictionary<T>();
        }
    }


    public class CellDataDictionary<T> : Dictionary<Vector2Int, T>
    {
        public void AddOrChange(Vector2Int cell, T value)
        {
            if (ContainsKey(cell))
            {
                this[cell] = value;
            }
            else
            {
                Add(cell, value);
            }
        }

        public T GetOrDefault(Vector2Int key, T @default)
        {
            return ContainsKey(key) ? this[key] : @default; 
        }

        public void AddOrChangeAll(IEnumerable<(Vector2Int cell, T value)> tups)
        {
            foreach (var valueTuple in tups)
            {
                AddOrChange(valueTuple.cell, valueTuple.value);
            }
        }
    }

   
}