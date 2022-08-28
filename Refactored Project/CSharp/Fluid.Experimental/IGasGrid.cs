using System;
using System.Collections.Generic;

namespace Game.Fluid.Experimental
{
    public interface IGasGrid
    {
        bool IsBlocked(int tileX, int tileY);
        int GetGas(int tileX, int tileY);
        void SetGas(int tileX, int tileY, int gasAmount);
    }


    /// <summary>
    /// simple wrapper for IntVector2 that is explicitly referencing a grid location.  This is more intuitive and explicit than accessing them with a more generic data structure,
    /// and it is easy to write implicit conversions (provided they don't created any hidden consequences, which they shouldn't since there is only primitive data types stored in this) 
    /// </summary>
    public struct Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static explicit operator (int x, int y)(Cell cell) => (cell.X, cell.Y);
        public static implicit operator Cell((int, int) tuple) => new Cell(tuple.Item1, tuple.Item2);
        public static implicit operator Cell((float, float) tuple) => new Cell((int)tuple.Item1, (int)tuple.Item2);
    }

    
    /// <summary>
    /// Generic Data container for data, this assumes that each cell can only store one instance per cell for any given data type.  To add additional data to a cell
    /// 2 options for extending functionality to allow cells to store additional data
    /// (1) define a new structure for that data
    /// (2) use GridData to store a collection of data at each location 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class GridData<T>
    {
        private static Dictionary<Cell, T> _data = new Dictionary<Cell, T>();
        

        public static bool HasDataAt(Cell cell)
        {
            throw new NotImplementedException();
            if (_data.ContainsKey(cell)) return true;
            return false;
        }

        public static T GetDataAt(Cell cell)
        {
            return default(T);
        }

        public static void SetDataAt(Cell cell)
        {
            throw new NotImplementedException();
        }

        public static bool HasDataAt(int tileX, int tileY)
        {
            return HasDataAt((tileX, tileY));
            // if (_data.ContainsKey((tileX, tileY))) return true;
            // return false;
        }

        public static T GetDataAt(int tileX, int tileY) => GetDataAt((tileX, tileY));

        public static void SetDataAt(int tileX, int tileY) => SetDataAt((tileX, tileY));
    }
    
    
        
    /// <summary>
    /// this is more of me playing with code than something actually usable. The idea was to store both a cell location and a grid id and use that grid id
    /// to access a grid transform which allows cell coordinate conversion between grids of different resolutions
    /// </summary>
    public struct GridCell {
        public Cell cell { get; }
        public int gridID { get; }

        public GridCell(int X, int Y, int id = 0)
        {
            this.gridID = id;
            this.cell = new Cell(X, Y);
        }
        
        public static implicit operator Cell(GridCell gridCell) => gridCell.cell;
    }

}
