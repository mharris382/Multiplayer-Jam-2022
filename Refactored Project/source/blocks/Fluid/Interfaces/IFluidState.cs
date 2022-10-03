using System;
using System.Collections.Generic;
using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    [Obsolete]
    public interface IFluidState
    {
        void SetCell(Vector2Int cell, int gasValue);

        IEnumerable<(Vector2Int, int)> GetCellStates();
    }
}