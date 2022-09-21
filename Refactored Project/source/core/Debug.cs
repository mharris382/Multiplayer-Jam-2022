using System;
using System.Text;
using Godot;

namespace Game.core
{
    public static class Debug
    {
        public static bool AssertNotNull<T>(this Node node, T value, bool withException = false)
        {
            if (value != null) return true;
            if (withException)
                throw new NullReferenceException($"Error the node {node.Name}({node.GetType()}) failed a null check on {typeof(T).Name}");
            GD.PrintErr($"Error the node {node.Name}({node.GetType()}) failed a null check on {typeof(T).Name}");
            return false;
        }
        public static bool AssertNotNull<T>(T value, bool withException = false)
        {
            if (value != null) return true;
            if (withException)
                throw new NullReferenceException($"{typeof(T).Name} must not be null");
            GD.PrintErr($"{typeof(T).Name} must not be null");
            return false;
        }

        public static void Log(params object[] message)
        {
            GD.Print(message);
        }

        public static void LogWarning(string msg)
        {
            GD.PushWarning(msg);
        }

        public static bool Assert(bool value, string msg)
        {
            if (value) return true;
            GD.PrintErr(msg);
            return false;
        }
    }
}