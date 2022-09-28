using System.Collections.Generic;
using Game.blocks.gas;
using Godot;
using Godot.Collections;

namespace Game.Blocks.Gas.Tilemaps
{
    public class GasTilemap : TileMap
    {
        private const int MAX_STEAM_VALUE = 16;


        [Export(hint: PropertyHint.ExpRange, hintString: "1,32,2")]
        public int steamTilesPerBlockTile = 4;

        [Export()] public NodePath pathToBlockTilemap = "./Block TileMap";


        public override void _Ready()
        {
            GasStuff.GasTilemap = this;
        
        }


        /// <summary>
        /// gets the steam value mapped to the given tileset tile index
        /// </summary>
        /// <param name="tileIndex">0-15</param>
        /// <returns>steam value [1-16]</returns>
        private static int TileIdToSteam(int tileIndex) => tileIndex + 1;
        
        
        /// <summary> gets the tileset tile index for a given steam value </summary>
        /// <param name="steamValue">should range from 1-16</param>
        /// <returns>tile set id [0-15]</returns>
        private static int SteamToTileId(int steamValue) => steamValue - 1;

        
        
        /// <summary> adds gas to the tilemap from a gas source </summary>
        /// <returns>amount of gas added</returns>
        public int AddGasFromSource(Vector2 cell, int gasAmount)
        {
            return ModifySteam(cell, gasAmount);
        }
        
        
        public int ModifySteam(Vector2 tilePosition, int amountToAdd)
        {
            var current = GetSteam(tilePosition);
            if (current + amountToAdd <= MAX_STEAM_VALUE)
            {
                SetSteam(tilePosition, current + amountToAdd);
                return amountToAdd;
            }
        
            var amountAdded = MAX_STEAM_VALUE - current;
            SetSteam(tilePosition, MAX_STEAM_VALUE);
            return amountAdded;
        }

        
        /// <summary>
        /// removes the amount provided there is that much to remove, otherwise removes all the steam
        /// TODO: make an AddSteam version of this method
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="amountToRemove"></param>
        /// <returns>returns the amount of steam that was removed</returns>
        public int RemoveSteam(Vector2 cell, int amountToRemove)
        {
            var amount = Mathf.Abs(amountToRemove); //accept negative input or positive
            var prevSteam = GetSteam(cell);

            if (prevSteam > amount)
            {
                SetSteam(cell, prevSteam - amount);
                return amount;
            }

            SetSteam(cell, 0);
            return prevSteam;
        }


        /// <summary>
        /// tries to transfer the desired amount of steam from one cell to another cell WITH conservation of mass.
        /// Returns true if ANY steam was transferred.
        /// NOTE: the amount transfer will be less than or equal to the transferAmount
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="transferAmount">desired amount to transfer, actual amount may be less</param>
        /// <returns>false only if no steam was moved</returns>
        private bool TransferSteam(Vector2 fromPosition, Vector2 toPosition, ref int transferAmount)
        {
            var fromGas = GetSteam(fromPosition);
            var toGas = GetSteam(toPosition);
            var amountCanAdd = 16 - toGas;
            var amountCanTake = fromGas;
            var amountCanMove = Mathf.Min(amountCanAdd, amountCanTake);
            var amount = Mathf.Min(transferAmount, amountCanMove);
            if (amount > 0)
            {
                var a1 = ModifySteam(fromPosition, -amount);
                ModifySteam(toPosition, amount);
            }

            transferAmount = amount;
            return amount > 0;
        }

        /// <summary>
        /// tries to transfer the given amount from a to b
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public int MoveSteam(Vector2 from, Vector2 to, int amt)
        {
            var fmAmount = from.GetGasAmount();
            var toAmount = to.GetGasAmount();
            var toSpace = 16 - toAmount;
            var toSupply = fmAmount;
            amt = Mathf.Clamp(amt, toSpace, toSupply);
            TransferSteam(from, to, ref amt);
            return amt;
        }

        private void SetSteam(int x, int y, int steamValue)
        {
            steamValue = Mathf.Clamp(steamValue, 0, MAX_STEAM_VALUE);
            var current = GetSteam(x, y);
            if (current != steamValue)
            {
                SetCell(x, y, SteamToTileId(steamValue));
            }
        }


        public void SetSteam(Vector2 tilePosition, int steamValue) =>
            SetSteam((int)tilePosition.x, (int)tilePosition.y, steamValue);


        private int GetSteam(int tileX, int tileY)
        {
            //TODO: this code will break if a tile index above 16 is used, which could easily happen
            return TileIdToSteam(GetCell(tileX, tileY));
        }

        
        public int GetSteam(Vector2 tilePosition) => GetSteam((int)tilePosition.x, (int)tilePosition.y);

        
        /// <summary>
        /// clears gas from given gas cells.  returns the amount of gas removed from the simulation.
        /// assumes all gas cells are valid
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int ClearCells(IEnumerable<Vector2> cells)
        {
            int cnt = 0;
            foreach (var cell in cells)
            {
                cnt += GetSteam(cell);
                SetCellv(cell, -1);
            }

            return cnt;
        }

        
        public new Array<Vector2> GetUsedCells() => new Array<Vector2>(base.GetUsedCells());
        
        public new Array<Vector2> GetUsedCellsById(int tileId) => new Array<Vector2>(base.GetUsedCellsById(tileId));
        
    }
}
