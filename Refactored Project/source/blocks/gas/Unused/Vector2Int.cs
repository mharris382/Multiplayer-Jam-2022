﻿using Godot;

namespace Game.Blocks.Gas
{
    public struct Vector2Int
    {
        public int x { get; set; }
    
        public int y { get; set; }

        public static Vector2Int left = new Vector2Int(-1, 0);
        public static Vector2Int right = new Vector2Int(1, 0);
        public static Vector2Int Up => new Vector2Int(0, -1);
        public static Vector2Int Down => new Vector2Int(0, 1);

        public static Vector2Int Zero { get; }

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

        public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
        {
            v1.x += v2.x;
            v1.y += v2.y;
            return v1;
        }
        
        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
        {
            v1.x -= v2.x;
            v1.y -= v2.y;
            return v1;
        }
        
        public static Vector2Int operator *(Vector2Int v1, int m)
        {
            v1.x *= m;
            v1.y *= m;
            return v1;
        }
        
        public static Vector2Int operator /(Vector2Int v1, int m)
        {
            v1.x /= m;
            v1.y /= m;
            return v1;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static bool operator ==(Vector2Int v1, Vector2Int v2) => v1.x == v2.x && v1.y == v2.y;

        public static bool operator !=(Vector2Int v1, Vector2Int v2) => v1.x != v2.x || v1.y != v2.y;

        public static implicit operator Vector2Int(Vector2 vector2) => new Vector2Int(vector2);
        public static implicit operator Vector2(Vector2Int vector2Int) => new Vector2(vector2Int.x, vector2Int.y);
    
    }
}
