using System.Collections.Generic;
using Godot;

namespace Game.core
{
    public static class VectorExtensions
    {
        public static Vector2 ClampXY(this Vector2 vector2, int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(Mathf.Clamp(vector2.x, minX, maxX),
                Mathf.Clamp(vector2.y, minY, maxY));
        }


        public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}