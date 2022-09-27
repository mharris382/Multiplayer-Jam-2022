using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using UnblockedNeighborsList = System.Collections.Generic.List<(Godot.Vector2 cell, int gasAmount)>;
using GasOutflowLookup = System.Collections.Generic.Dictionary<Godot.Vector2, int>;
using GasOutflowRecord = System.Collections.Generic.Dictionary<Godot.Vector2, System.Collections.Generic.Dictionary<Godot.Vector2, int>>;

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




    }
}