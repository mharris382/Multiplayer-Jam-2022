using Godot;

namespace Game.Testing
{
    public interface ITileMap : IReadOnlyTileMap
    {
        void SetCell(int x, int y, int tile);
        void SetCell(Vector2 location, int tile);
    }
}