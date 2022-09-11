using Godot;

public struct PositionVector
{
    public enum LocationSpace
    {
        WorldSpace,
        GasSpace,
        BlockSpace
    }
    
    public Vector2 Position { get; private set; }

    public LocationSpace Space { get; set; }

    public PositionVector(Vector2 pos, LocationSpace space = LocationSpace.WorldSpace)
    {
        this.Position = pos;
        this.Space = space;
    }
    
}