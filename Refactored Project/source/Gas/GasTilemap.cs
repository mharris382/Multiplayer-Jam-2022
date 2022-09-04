using Game.Testing;
using Godot;
using Godot.Collections;

namespace Game.Gas
{
    public class GasTilemap : TileMap, ISteamMap, ITileMap
    {
        
        private const int MAX_STEAM_VALUE = 16;

        private GdTilemap _tilemap;
        
        public override void _Ready()
        {
            _tilemap = new GdTilemap(this);
        }

        public bool ModifySteam(Vector2 tilePosition, int amountToAdd, out int amountAdded)
        {
            var current = GetSteam(tilePosition);
            if (current + amountToAdd <= MAX_STEAM_VALUE)
            {
                SetSteam(tilePosition, current+amountToAdd);
                amountAdded = amountToAdd;
                return true;
            }
            throw new System.NotImplementedException();
        }

        public int GetSteam(int tileX, int tileY)
        {
            //TODO: this code will break if a tile index above 16 is used, which could easily happen
            return TileIdToSteam(GetCell(tileX, tileY));
        }

        public int GetSteam(Vector2 tilePosition)
        {
            return GetSteam((int)tilePosition.x, (int)tilePosition.y);
        }

        private void SetSteam(int  x, int y, int steamValue)
        {
            steamValue = Mathf.Clamp(steamValue, 0, MAX_STEAM_VALUE);
            var current = GetSteam(x, y);
            if (current != steamValue)
            {
                SetCell(x, y, SteamToTileId(steamValue));
            }
        }

        private void SetSteam(Vector2 tilePosition, int steamValue)
        {
            SetSteam((int)tilePosition.x, (int)tilePosition.y, steamValue);
        }

        private static int TileIdToSteam(int tileIndex) => tileIndex + 1;
        private static int SteamToTileId(int steamValue) => steamValue - 1;

        public int GetCell(Vector2 location)
        {
            return _tilemap.GetCell(location);
        }

        public Vector2 MapToWorld(Vector2 gridSpaceLocation)
        {
            return _tilemap.MapToWorld(gridSpaceLocation);
        }

        public new Array<Vector2> GetUsedCellsById(int tileId)
        {
            return _tilemap.GetUsedCellsById(tileId);
        }

        public new Array<Vector2> GetUsedCells()
        {
            return _tilemap.GetUsedCells();
        }

        public void SetCell(int x, int y, int tile)
        {
            _tilemap.SetCell(x, y, tile);
        }

        public void SetCell(Vector2 location, int tile)
        {
            _tilemap.SetCell(location, tile);
        }
    }
}
