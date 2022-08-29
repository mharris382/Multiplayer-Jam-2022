
namespace GasSimulation.Gas
{
    public struct CellVector
    {

        public CellVector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }


        public static implicit operator CellVector((int x, int y) tuple) => new CellVector(tuple.x, tuple.y);
        public static implicit operator (int x, int y)(CellVector gvCellVector) => (gvCellVector.x, gvCellVector.y);

        public static implicit operator CellVector(Godot.Vector2 vector2) =>
            new CellVector((int)vector2.x, (int)vector2.y);
    }

    public struct Vector
    {
        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float x { get; set; }
        public float y { get; set; }

#if GODOT
        public static implicit operator Vector(Godot.Vector2 vector2) =>
            new Vector(vector2.x, vector2.y);

        public static implicit operator Godot.Vector2(Vector v) => new Godot.Vector2(v.x, v.y);
#elif UNITY_
        public static implicit operator Vector(UnityEngine.Vector2 vector2) =>
            new Vector(vector2.x, vector2.y);

        public static implicit operator UnityEngine.Vector2(Vector v) => new UnityEngine.Vector2(v.x, v.y);
#endif
    }
}