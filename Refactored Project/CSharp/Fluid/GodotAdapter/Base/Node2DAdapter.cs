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

        public Vector2Int GetTilemapCell(TileMap tileMap)
        {
            var tileLocation = tileMap.WorldToMap(GlobalPosition);
            return (Vector2Int)tileLocation;
        }
    }
}
