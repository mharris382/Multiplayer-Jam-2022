using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot.Collections;



public class GasTilemap : TileMap
{
    private const int MAX_STEAM_VALUE = 16;
    
    
    [Export(hint:PropertyHint.ExpRange, hintString:"1,32,2")]
    public int steamTilesPerBlockTile = 4;

    [Export()]
    public NodePath pathToBlockTilemap = "./Block TileMap";

    
  
    
    public override void _Ready()
    {
        GasStuff.GasTilemap = this;
    }

    private static int TileIdToSteam(int tileIndex) => tileIndex + 1;
    private static int SteamToTileId(int steamValue) => steamValue - 1;
    
    public bool ModifySteam(Vector2 tilePosition, int amountToAdd, out int amountAdded)
    {
        var current = GetSteam(tilePosition);
        if (current + amountToAdd <= MAX_STEAM_VALUE)
        {
            SetSteam(tilePosition, current+amountToAdd);
            amountAdded = amountToAdd;
            return true;
        }
        amountAdded = GetSteam(tilePosition) - current;
        return false;
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
    public bool TransferSteam(Vector2 fromPosition, Vector2 toPosition, ref int transferAmount)
    {
        var fromGas = GetSteam(fromPosition);
        var toGas = GetSteam(toPosition);
        var amountCanAdd = 16 - toGas;
        var amountCanTake = fromGas;
        var amountCanMove = Mathf.Min(amountCanAdd, amountCanTake);
        var amount = Mathf.Min(transferAmount, amountCanMove);
        if (amount > 0)
        {
           ModifySteam(fromPosition, -amount, out var added1);
           ModifySteam(toPosition, amount, out var added2);
        }
        transferAmount = amount;
        return amount > 0;
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

    public IEnumerable<Vector2> GetNeighbors(Vector2 pos)
    {
        yield return pos + Vector2.Up;
        yield return pos + Vector2.Down;
        yield return pos + Vector2.Left;
        yield return pos + Vector2.Right;
    }

    public IEnumerable<Vector2> GetLowerNeighbors(Vector2 pos)
    {
        var gasAmount = GetSteam(pos);
        foreach (var neighbor in GetNeighbors(pos))
        {
            var neighborGasAmount = GetSteam(neighbor);
            yield return neighbor;
            Debug.Log($"Found Neighbor at {neighbor}");
        }
    }

    private void SetSteam(Vector2 tilePosition, int steamValue) => SetSteam((int)tilePosition.x, (int)tilePosition.y, steamValue);

    public int GetSteam(int tileX, int tileY)
    {
        //TODO: this code will break if a tile index above 16 is used, which could easily happen
        return TileIdToSteam(GetCell(tileX, tileY));
    }

    public int GetSteam(Vector2 tilePosition)
    {
        return GetSteam((int)tilePosition.x, (int)tilePosition.y);
    }
    
    
    public new Array<Vector2> GetUsedCells() => new Array<Vector2>(base.GetUsedCells());
    public new Array<Vector2> GetUsedCellsById(int tileId) => new Array<Vector2>(base.GetUsedCellsById(tileId));
}
