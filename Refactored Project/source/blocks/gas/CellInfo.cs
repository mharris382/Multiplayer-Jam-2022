using System;
using Game.blocks.gas;
using Godot;

public struct CellInfo
{
    private readonly Vector2 _cellPosition;
    private int _gas;
    
    public int Gas
    {
        get => _gas;
        set => _gas = Mathf.Clamp(value, -1, 16);
    }
    
    public CellInfo(Vector2 cellPosition, int gas)
    {
        _cellPosition = cellPosition;
        _gas = gas;
    }

    public override int GetHashCode()
    {
        return _cellPosition.GetHashCode();
    }

    public static implicit operator CellInfo(Vector2 vector2)
    {
        var gas = GasStuff.IsGasCellBlocked(vector2) ? -1 : vector2.GetGasAmount();
        return new CellInfo(vector2, gas);
    }

    public static implicit operator Vector2(CellInfo cellInfo)
    {
        return cellInfo._cellPosition;
    }

    public static implicit operator int(CellInfo cellInfo)
    {
        return cellInfo._gas;
    }
}