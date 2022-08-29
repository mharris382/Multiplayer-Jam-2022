using Godot;

namespace GasSimulation.Gas
{
    public class GasTileMap : TileMap
    {
        private const int MAX_STEAM_VALUE = 16;

        public bool ModifySteam(Vector2 tilePosition, int amountToAdd, out int amountAdded)
        {
            var current = GetSteam(tilePosition);
            var minAmount = -current;
            var maxAmount = MAX_STEAM_VALUE - current;
            amountAdded = Mathf.Clamp(amountToAdd, minAmount, maxAmount);
            amountToAdd = amountAdded;
            if (current + amountToAdd <= MAX_STEAM_VALUE )
            {
                SetSteam(tilePosition, current+amountToAdd);
                amountAdded = amountToAdd;
                return true;
            }
            throw new System.NotImplementedException();
        }

        public int CalculateAmountWillBeModified(Vector tilePos, int amountToAdd)
        {
            var prev = GetSteam(tilePos);
            int actual = 0;
            ModifySteam(tilePos, amountToAdd, out actual);
            SetSteam(tilePos, prev);
            return actual;
        }
        public int ModifySteam(Vector2 pos, int amount)
        {
            int actual = 0;
            ModifySteam(pos, amount, out actual);
            return actual;
        }
        public int this[(int x, int y) location]
        {
            get => GetSteam(location.x, location.y);
            set => SetSteam(location.x, location.y , value);
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

        public void SetSteam(int  x, int y, int steamValue)
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
        
        
    }
}
