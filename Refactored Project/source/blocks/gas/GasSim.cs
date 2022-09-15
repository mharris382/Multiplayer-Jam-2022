using System.Collections.Generic;
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
    }
}