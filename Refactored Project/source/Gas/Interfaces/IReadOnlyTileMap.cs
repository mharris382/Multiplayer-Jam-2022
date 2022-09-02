using Godot;

namespace Game.Testing
{
    public interface IReadOnlyTileMap
    {
        int GetCell(int x, int y);
        int GetCell(Vector2 location);

        Vector2 WorldToMap(Vector2 worldSpaceLocation);
        Vector2 MapToWorld(Vector2 gridSpaceLocation);

        Godot.Collections.Array<Vector2> GetUsedCellsById(int tileId);
        Godot.Collections.Array<Vector2> GetUsedCells();
    }
}