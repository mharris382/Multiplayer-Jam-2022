using System;
using System.Collections.Generic;
using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public enum CellWriteMode
    {
        /// <summary>replaces the given cells values on the grid and but leaves other cells unchanged </summary>
        OVERWRITE,
        /// <summary> replaces the given cells values on the grid and sets all others to zero </summary>
        OVERWRITE_CLEAR,
        /// <summary> tries to add the given cell values with the current cell values stored on the grid </summary>
        BLEND_ADD
    }
    public interface IFluid
    {
        void WriteCells(IEnumerable<(Vector2Int gasCell, int pressure)> cellsToWrite, CellWriteMode mode);
        IEnumerable<(Vector2Int cell, int pressure)> GetUseCellStates();

        event Action<Vector2Int> OnCellClearedOfGas;

    }

    

    public interface IGasPressureIterator
    {
        IEnumerable<(Vector2Int, int)> GetAllNonEmptyCells();
        IEnumerable<(Vector2Int, int)> GetAllNonEmptyCellsDescending();
    }
    
    
    
}