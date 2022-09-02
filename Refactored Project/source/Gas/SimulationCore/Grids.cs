namespace Game.Gas.SimulationCore
{
    public static class Grids
    {
        
        
        
    }

    public struct GridCell
    {
        public readonly GridInfo grid;
        
        public int x { get; set; }
        public int y { get; set; }
        
#if GODOT
        public Godot.Vector2 GridPosition
        {
            get => new Godot.Vector2(x, y);
            set
            {
                x = (int)value.x;
                y = (int)value.y;
            }
        }
#endif
    }
}