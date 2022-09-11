using System;
using Godot;

namespace Game.blocks.gas
{
    public class GasSimulation
    {
        public readonly Vector2Int wsBoundsMin;
        public readonly Vector2Int wsBoundsMax;

        private Func<Vector2, int> getGasFunc;
        private Func<Vector2, int> setGasFunc;

        public GasSimulation(Bounds gasGridBounds)
        {
            
        }

        public struct Bounds
        {
            public readonly Vector2Int wsBoundsMin;
            public readonly Vector2Int wsBoundsMax;
            
            public Bounds(int xMin=0, int yMin=0, int xMax =100, int yMax=100)
            {
                wsBoundsMax = new Vector2Int(xMax, yMax);
                wsBoundsMin = new Vector2Int(xMin, yMin);
            }
            public Bounds(Vector2 boundsMin, Vector2 boundsMax)
            {
                wsBoundsMax = boundsMax;
                wsBoundsMin = boundsMin;
            }
        }
    }

    public struct GasCell
    {
        private Vector2Int cellCoordinate;
        
        public GasCell(PositionVector positionVector)
        {
            cellCoordinate = positionVector.ToGasSpace();
        }
    }
}