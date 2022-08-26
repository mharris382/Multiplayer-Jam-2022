namespace Game.Fluid.Core
{
    public struct GasCell
    {
        public int X;
        public int Y;

        public BlockCell GetBlockCell()
        {
            return new BlockCell(this.X / GlobalFluidSettings.AirToBlockTileScale,
                this.Y / GlobalFluidSettings.AirToBlockTileScale);
        }

        public GasCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}