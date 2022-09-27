namespace Game.Blocks.Gas
{
    public interface IGasTransferHandler
    {

        int TransferGas(Vector2Int fromCell, Vector2Int toCell, int amount);
    }

    public interface IGasGrid
    {
        
        bool IsCellBlocked(Vector2Int cell);
        int GetCellGas(Vector2Int cell);
    }

    
}