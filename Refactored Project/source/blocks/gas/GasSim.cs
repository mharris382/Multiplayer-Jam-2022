using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Game.blocks.gas
{
    public static class GasSim
    {

        public static bool HasEmptyNeighbor(IEnumerable<(Vector2, int)> neighbors, out Vector2 emptyNeighbor)
        {
            var glist = new Godot.Collections.Array<(Vector2, int)>(neighbors);
            glist.Shuffle();
            foreach (var valueTuple in glist)
            {
                if (valueTuple.Item2 == 0)
                {
                    emptyNeighbor = valueTuple.Item1;
                    return true;
                }
            }

            emptyNeighbor = Vector2.Zero;
            return false;
        }

        public static int GetEmptyNeighbors(IEnumerable<(Vector2, int)> neighbors,
            out Godot.Collections.Array<Godot.Vector2> emptyNeighbors)
        {
            emptyNeighbors = new Array<Vector2>();
            int cnt = 0;
            foreach (var valueTuple in neighbors)
            {
                if (valueTuple.Item2 == 0)
                {
                    emptyNeighbors.Add(valueTuple.Item1);
                    cnt++;
                }
            }

            return cnt;
        }


        private static readonly System.Collections.Generic.Dictionary<Vector2, CellHandle> CellHandles =
            new System.Collections.Generic.Dictionary<Vector2, CellHandle>();

        //TODO: move to extensions class
         public static int GetGasAmount(this Vector2 cellPosition)
         {
            if (CellHandles.TryGetValue(cellPosition, out var value))
            {
                return value.GasAmount;
            }

            CellHandles.Add(cellPosition, new CellHandle(cellPosition));
            return CellHandles[cellPosition].GasAmount;
         }
         
        public static CellHandle GetCellHandle(this Vector2 cellPosition)
        {
            if (CellHandles.TryGetValue(cellPosition, out var value))
            {
                return value;
            }

            CellHandles.Add(cellPosition, new CellHandle(cellPosition));
            return CellHandles[cellPosition];
        }

        public static int GetLowerNeighbors(int cellGasAmount, List<(Vector2 cell, int gasAmount)> neighbors,
            out Array<Vector2> lowerNeighbors)
        {
            lowerNeighbors = new Array<Vector2>();
            int cnt = 0;
            foreach (var valueTuple in neighbors)
            {
                if (valueTuple.Item2 < cellGasAmount)
                {
                    lowerNeighbors.Add(valueTuple.Item1);
                    cnt++;
                }
            }

            return cnt;
        }

        
        
        // private static List<Vector2> _neighbors = new List<Vector2>(4);
        // public static int GetLowerNeighbors(Vector2 cell, out Array<Vector2> lowerNeighbors, CellSortMode sortMode = CellSortMode.NONE)
        // {
        //     var cellHandle = cell.GetCellHandle();
        //     int cellGasAmount = cellHandle.GasAmount;
        //     var lNeighbors = cellHandle.LowerNeighbors;
        //     _neighbors.Clear();
        //     _neighbors.AddRange(lNeighbors.Select(t=> t.Position));
        //
        //     switch (sortMode)
        //     {
        //         case CellSortMode.NONE:
        //             break;
        //         case CellSortMode.GAS_DESCENDING:
        //             _neighbors.Sort(new DescendingOrderGasSorter());
        //             break;
        //         case CellSortMode.GAS_ASCENDING:
        //             throw new NotImplementedException();
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(sortMode), sortMode, null);
        //     }
        //     
        //     lowerNeighbors = new Array<Vector2>(_neighbors);
        //     return _neighbors.Count;
        // }
    }

    #region [Sorting Stuff]
    
    // /// <summary>
    // /// <seealso cref="GasStuff.GetLowerNeighbors"/>
    // /// </summary>
    // public enum CellSortMode
    // {
    //     NONE,
    //     GAS_DESCENDING,
    //     GAS_ASCENDING
    // }
    //
    public abstract class GasCellSorter : IComparer<Vector2>
    {
        public int Compare(Vector2 x, Vector2 y)
        {
            return Compare(x.GetCellHandle(), y.GetCellHandle());
        }
    
        protected abstract int Compare(CellHandle x, CellHandle y);
    }
    
    public class DescendingOrderGasSorter : GasCellSorter
    {
        protected override int Compare(CellHandle x, CellHandle y)
        {
            if (x.IsBlocked)
                return 1;
            if (y.IsBlocked)
                return -1;
            var xGas = x.GasAmount;
            var yGas = y.GasAmount;
            if (xGas < yGas) return -1;
            else if (yGas > xGas) return 1;
            else return 0;
        }
    }
    
    #endregion
}

