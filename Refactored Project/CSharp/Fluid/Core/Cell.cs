using Godot;

namespace Game.Fluid.Core
{
    public struct Cell
    {
        public int x { get; set; }
        public int y { get; set; }
        
        
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        
#if GODOT
        public static implicit operator Vector2(Cell cell)
        {
            return new Vector2(cell.x, cell.y);
        }

        public static explicit operator Cell(Vector2 vector2)
        {
            return new Cell(
                Mathf.RoundToInt(vector2.x), 
                Mathf.RoundToInt(vector2.y));
        }
#endif
    }
}