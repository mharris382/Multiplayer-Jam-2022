using Godot;
using System;
public class SolidBlockTilemap : TileMap
{
    
    
    public override void _Ready()
    {
        GasStuff.BlockTilemap = this;
    }
}
