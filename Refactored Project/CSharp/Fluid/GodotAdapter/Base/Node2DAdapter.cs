using Game.Fluid.Core;
using Godot;

namespace Game.Fluid.GodotAdapter
{
    public class Node2DAdapter : Node2D
    {
        
        public override void _Ready()
        {
            GD.Print($"{this.GetType().Name} Ready");
        }

        public Cell GetTilemapCell(TileMap tileMap)
        {
            var tileLocation = tileMap.WorldToMap(GlobalPosition);
            return (Cell)tileLocation;
        }
    }
}
