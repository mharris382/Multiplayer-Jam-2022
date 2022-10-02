using System.Collections.Generic;
using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public struct CellSink
    {

        private Vector2Int gasPosition;
        private byte rate;

        public Vector2Int GasPosition => gasPosition;
        public int Rate => rate;

        public CellSink(Vector2Int gasCellPosition, byte rate)
        {
            this.gasPosition = gasCellPosition;
            this.rate = rate;
        }
    }
}