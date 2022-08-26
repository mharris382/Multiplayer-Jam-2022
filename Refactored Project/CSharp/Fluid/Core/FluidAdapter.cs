using Godot;

namespace Game.Fluid.Core
{
    public interface IFluidAdapter
    {
        bool IsCellBlocked(Vector2Int vector2Int);
        int GetGas(Vector2Int vector2Int);
        void SetGas(Vector2Int vector2Int, int amount);
        
    }
    
}