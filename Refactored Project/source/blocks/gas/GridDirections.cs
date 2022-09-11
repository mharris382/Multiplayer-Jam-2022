using System;

namespace Game.blocks.gas
{
    /// <summary>
    /// this enum encapsulates an encoding to store a set of discrete (grid) directions, currently only stores 4 directions,
    /// but could be extended to support diagonals as well
    /// </summary>
    [Flags]
    public enum GridDirections
    {
        NONE = 0,
        RIGHT = 1 << 0,
        LEFT = 1 << 1,
        UP = 1 << 2,
        DOWN = 1 << 3,
        ALL = RIGHT | LEFT | UP | DOWN
    }
}