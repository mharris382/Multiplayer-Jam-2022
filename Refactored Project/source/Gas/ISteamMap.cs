using Godot;

namespace Game.Gas
{
    public interface ISteamMap
    {
        int GetSteam(int tileX, int tileY);
        int GetSteam(Vector2 tilePosition);

        bool ModifySteam(Vector2 tilePosition, int amountToAdd, out int amountAdded);
    }
}