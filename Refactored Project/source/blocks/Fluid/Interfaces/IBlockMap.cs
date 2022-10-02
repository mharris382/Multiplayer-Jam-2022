using System;
using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public interface IBlockMap
    {
        /// <summary>
        /// returns the corresponding block cell coordinates, given a gas cell coordinate.  This method assumes that
        /// block cells are larger than gas cells. 
        /// </summary>
        /// <param name="gasCell">coordinate corresponding to a gas cell</param>
        /// <returns></returns>
        Vector2Int ConvertToBlockCell(Vector2Int gasCell);

        bool IsCellBlocked(Vector2Int blockCell);
        
        event Action<Vector2Int> BlockCellAdded;
        event Action<Vector2Int> BlockCellRemoved;
    }
}