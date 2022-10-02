using System.Collections.Generic;
using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public interface IGasToBlockConverter
    {
        int GasToBlock { get; }
        Vector2Int GasToBlockCell(Vector2Int gasCell);
        
        IEnumerable<Vector2Int> BlockToGasCells(Vector2Int blockCell);
    }
}