
using System;
#if GODOT
using Godot;
#elif UNITY
using UnityEngine;
#endif

namespace GasSimulation.Gas
{
    public static class MathF
    {

        public static float Pi
        {
            get
            {
#if GODOT
                return Godot.Mathf.Pi;
#elif UNITY
                return UnityEngine.PI;
#endif
            }
        }

        public static float Infinity  { get=>
#if GODOT
            Mathf. Inf;
#elif UNITY
            => Mathf.Infinity;
#endif
        }

        public static float Sin(float r) => Mathf.Sin(r);
        public static float Cos(float r) => Mathf.Cos(r);
        public static float Atan2(float y, float x) => Mathf.Atan2(y, x);
        
        public static float Abs(float v) => Mathf.Abs(v);
        public static float Sign(float v) => Mathf.Sign(v);

        public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);

        public static int FloorToInt(float f) => Mathf.FloorToInt(f);
        public static int CeilToInt(float f) => Mathf.CeilToInt(f);
        public static int RoundToInt(float f) => Mathf.RoundToInt(f);

        public static float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);

        public static float InverseLerp(float a, float b, float t) => Mathf.InverseLerp(a, b, t);

        public static float Remap(float value, float aMin, float aMax, float bMin, float bMax)
        {
            
#if GODOT
            return Mathf.RangeLerp(value, aMin, aMax, bMin, bMax);
#elif UNITY
            var t = InverseLerp(aMin, aMax, value);
            return Lerp(bMin, bMax, t);
#endif
        }

        public static float Min(float a, float b, params float[] args)
        {
#if GODOT
            var curMin = Mathf.Min(a, b);
            if (args == null || args.Length == 0)
                return curMin;
            foreach (var f in args)
            {
                curMin = Mathf.Min(curMin, f);
            }
            return curMin;
#elif UNITY
            throw new System.NotImplementedException();
#endif
        }
        
        public static float Max(float a, float b, params float[] args)
        {
#if GODOT
            var curMax = Mathf.Max(a, b);
            if (args == null || args.Length == 0)
                return curMax;
            foreach (var f in args)
            {
                curMax = Mathf.Max(curMax, f);
            }
            return curMax;
#elif UNITY
            throw new System.NotImplementedException();
#endif
        }
    }
}