using Game.Blocks.Gas;
using Godot;

namespace Game.Blocks.Fluid
{
    public struct FluidCell
    {
        public Vector2Int Position;
        public byte density;
        public byte xVelocity;
        public byte yVelocity;

        public override string ToString() => $"{Position}: Density={density}, Velocity=({xVelocity}, {yVelocity})";

        public override int GetHashCode()
        {
            return (Position.x * 541) + (Position.y * 79);
        }
    }
    
   
}