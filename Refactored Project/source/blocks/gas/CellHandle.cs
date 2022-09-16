using System;
using System.Collections.Generic;
using System.Linq;
using Game.blocks.gas;
using Game.core;
using Godot;

/// <summary>
/// used to make performing actions on a cell more explicit and intuitive to read and write
/// </summary>
public struct CellHandle
{
    private const int MIN_CELL_X = -200;
    private const int MIN_CELL_Y = -200;
    private const int MAX_CELL_X = 1000000;
    private const int MAX_CELL_Y = 1000000;
    
    
    
    private Vector2 position;
    
    public Vector2 Position => position;

    
    public CellHandle(Vector2 gasCoord)
    {
        this.position = gasCoord.ClampXY(MIN_CELL_X, MAX_CELL_X, 
            MIN_CELL_Y, MAX_CELL_Y);
    }

    public bool IsBlocked => GasStuff.IsGasCellBlocked(position);
    public bool IsValid => GasStuff.GasTilemap != null;
    
    public int GasAmount
    {
        get
        {
            if (!IsValid || IsBlocked) return int.MinValue;
            return GasStuff.GasTilemap.GetSteam(position);
        }
        set
        {
            if (!IsValid || IsBlocked) return;
            value = Mathf.Clamp(value, 0, 16);
            GasStuff.GasTilemap.SetSteam(position, value);
        }
    }

    public IEnumerable<CellHandle> UnblockedNeighbors
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IEnumerable<CellHandle> LowerNeighbors => _GetLowerNeighbors();


    private IEnumerable<CellHandle> _GetBlockedNeighbors() => _GetNeighbors().Where(t => t.IsBlocked);

    private IEnumerable<CellHandle> _GetUnblockedNeighbors() => _GetNeighbors().Where(t => !t.IsBlocked);

    private IEnumerable<CellHandle> _GetNeighbors()
    {
        return GasStuff.GetUnblockedNeighbors(this.position).Select(t => t.cell.GetCellHandle());
    }

    private IEnumerable<CellHandle> _GetEmptyNeighbors() => _GetUnblockedNeighbors().Where(t => t.GasAmount == 0);
    
    private IEnumerable<CellHandle> _GetLowerNeighbors()
    {
        foreach (var unblockedNeighbor in UnblockedNeighbors)
        {
            if (unblockedNeighbor.GasAmount < this.GasAmount)
            {
                yield return unblockedNeighbor;
            }
        }
    }
    
    private IEnumerable<CellHandle> _GetLowerNeighbors(int minimumDifference)
    {
        minimumDifference = Mathf.Clamp(minimumDifference, 0, 16);
        foreach (var unblockedNeighbor in UnblockedNeighbors)
        {
            var diff = unblockedNeighbor.GasAmount - this.GasAmount;
            if (GasAmount - unblockedNeighbor.GasAmount > diff)
            {
                yield return unblockedNeighbor;
            }
        }
    }

    private IEnumerable<CellHandle> _GetUnequalNeighbors()
    {
        foreach (var unblockedNeighbor in UnblockedNeighbors)
        {
            if (unblockedNeighbor.GasAmount != this.GasAmount)
            {
                yield return unblockedNeighbor;
            }
        }
    }
    // private IEnumerable<CellHandle> _GetUnblockedNeighbors()
    // {
    //     
    // }
    //public static int GetEmptyNeighbors()
}