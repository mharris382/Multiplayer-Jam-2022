using Godot;

namespace Game.Blocks.Gas
{
    public struct Vector2Int
    {
        public int x { get; set; }
    
        public int y { get; set; }

        public Vector2Int(Vector2 vector2)
        {
            this.x = Mathf.RoundToInt(vector2.x);
            this.y = Mathf.RoundToInt(vector2.y);
        }

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2Int(Vector2 vector2) => new Vector2Int(vector2);
        public static implicit operator Vector2(Vector2Int vector2Int) => new Vector2(vector2Int.x, vector2Int.y);
    
    }
}
