using System;
using Godot;

namespace GasSimulation.Gas
{
    public struct GridProperties
    {
        private const int DEFAULT_PIXELS_PER_CELL = 128;
        private const int HALF_SIZE_PPC = 64;
        private const int QUARTER_SIZE_PPC = 32;
        private const int DOUBLE_SIZE_PPC = 256;
        
        public readonly int PixelsPerCell;
        public readonly GridResolution GridResolution;

        public GridProperties(TileMap tileMap)
        {
            if (tileMap.CellSize.x != tileMap.CellSize.y)
            {
                throw new Exception("CellVector.GridProperties: Cell size mismatch");
            }
            
            PixelsPerCell = (int)(tileMap.CellSize.x);
            switch (PixelsPerCell)
            {
                case 32:
                    GridResolution = GridResolution.QuarterCellSize;
                    tileMap.CellSize = new Vector2(QUARTER_SIZE_PPC, QUARTER_SIZE_PPC);
                    break;
                case 64:
                    GridResolution = GridResolution.HalfCellSize;
                    tileMap.CellSize = new Vector2(HALF_SIZE_PPC, HALF_SIZE_PPC);
                    break;
                case 128:
                    GridResolution = GridResolution.StandardCellSize;
                    tileMap.CellSize = new Vector2(DEFAULT_PIXELS_PER_CELL, DEFAULT_PIXELS_PER_CELL);
                    break;
                case 256:
                    GridResolution = GridResolution.DoubleCellSize;
                    tileMap.CellSize = new Vector2(DOUBLE_SIZE_PPC, DOUBLE_SIZE_PPC);
                    break;
                default:
                    GridResolution = GridResolution.InvalidCellSize;
                    throw new Exception($"CellVector.GridProperties: invalid Cell size {PixelsPerCell}");
            }
        }
    }
}