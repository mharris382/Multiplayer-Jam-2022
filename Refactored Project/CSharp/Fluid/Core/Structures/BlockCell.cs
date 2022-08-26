namespace Game.Fluid.Core
{
    public struct BlockCell
    {
        public int X;
        public int Y;
        
        /*public GasCell GasCell => new */

        public BlockCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}