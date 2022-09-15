using System.Text;
using Godot;

namespace Game.core
{
    public static class Debug
    {
        public static bool AssertNotNull<T>(T value)
        {
            if (value != null) return true;
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