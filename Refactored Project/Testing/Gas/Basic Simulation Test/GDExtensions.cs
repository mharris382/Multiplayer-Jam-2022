using Godot;

namespace Game.Testing
{
    public static class GdExtensions
    {
        public static bool ThisIsNotNull(this Node node, string msg)
        {
            if (node == null)
            {
                GD.PrintErr(msg);
                return false;
            }

            return true;
        }
        public static bool Assert(this Node node, bool condition, string msg)
        {
            if (!condition)
            {
                GD.PrintErr(msg);
                return false;
            }
            return true;
        }
    }
}