using Godot;

namespace Game.blocks.gas
{
    public struct GasParticle
    {
        public Vector2 Cell { get; set; }
        
        /// <summary>
        /// discrete representation of air density.  air value ranges between 1-16.  
        /// <para>
        /// 0 is a special value representing no air.
        /// 1 is the minimum amount of air that fits into a cell.  At this value the air is considered to have no pressure.
        /// 16 is the maximum air which is allowed in a cell.  This is the maximum air pressure allowed in the cell.
        /// </para>
        /// </summary>
        public int AirDensity { get; private set; }
        
        private GasParticle(int x, int y)
        {
            Cell = new Vector2(x, y);
            AirDensity = 1;
        }
        
        
    }
}