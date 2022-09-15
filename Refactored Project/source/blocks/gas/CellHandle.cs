using System;
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

  
}