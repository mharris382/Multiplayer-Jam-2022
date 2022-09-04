using System;
using System.Collections.Generic;

namespace Game.Gas.SimulationCore
{
    public struct GasCell
    {
        public int gridX;
        public int gridY;
        
    }

    public struct GridInfo
    {
        public readonly int gridId;
        public readonly string gridName;
        
        /// <summary>
        /// measured in pixels
        /// </summary>
        public readonly int cellSize;

        public override int GetHashCode()
        {
            return gridId;
        }

        public override string ToString()
        {
            return gridName;
        }

        public static bool GridSizesFitTogether(GridInfo grid1, GridInfo grid2)
        {
            if (grid1.cellSize == grid2.cellSize) 
                return true;
            
            GridInfo largeGrid = grid1.cellSize > grid2.cellSize ? grid1 : grid2;
            GridInfo smallGrid = grid1.cellSize < grid2.cellSize ? grid1 : grid2;
            var div = largeGrid.cellSize / smallGrid.cellSize;
            return largeGrid.cellSize % div == 0;
        }
    }

    public static class Grid
    {
        private static Dictionary<string, int> _nameToIdLookup;
        private static Dictionary<int, GridInfo> _registeredGrids;
        
        

        static Grid()
        {
            _nameToIdLookup = new Dictionary<string, int>();
            _registeredGrids = new Dictionary<int, GridInfo>();
        }

        public static void RegisterGrid(string name, int cellSize)
        {
            
        }
    }
}