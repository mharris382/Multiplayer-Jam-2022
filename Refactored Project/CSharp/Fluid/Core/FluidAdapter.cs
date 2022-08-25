using Godot;

namespace Game.Fluid.Core
{
    public interface IFluidAdapter
    {
        bool IsCellBlocked(Cell cell);
        int GetGas(Cell cell);
        void SetGas(Cell cell, int amount);
        
    }
    
}