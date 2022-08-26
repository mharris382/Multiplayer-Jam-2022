
namespace Game.Fluid.Core
{
    public struct Vector2Int
    {
        public int x { get; set; }
        public int y { get; set; }
        
        
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        
#if GODOT
        public static implicit operator Godot.Vector2(Vector2Int vector2Int)
        {
            return new Godot.Vector2(vector2Int.x, vector2Int.y);
        }

        public static explicit operator Vector2Int(Godot.Vector2 vector2)
        {
            return new Vector2Int(
                Godot.Mathf.RoundToInt(vector2.x), 
                Godot.Mathf.RoundToInt(vector2.y));
        }
        
#elif UNITY_5_3_OR_NEWER
         public static implicit operator UnityEngine.Vector2(Vector2Int vector2Int)
        {
            return new UnityEngine.Vector2(vector2Int.x, vector2Int.y);
        }

        public static explicit operator Vector2Int(UnityEngine.Vector2 vector2)
        {
            return new Vector2Int(
                UnityEngine.Mathf.RoundToInt(vector2.x), 
                UnityEngine.Mathf.RoundToInt(vector2.y));
        }
#endif
    }
}