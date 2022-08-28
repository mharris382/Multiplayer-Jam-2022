using Godot;

namespace Game.Gas
{
    public class SteamSupplierNode : Node2D
    {
        private int capacity = 16;
        private int flowRate = 3;
        
        
        private SteamSupplier _supplier;
        
        private SteamSupplier Supplier => _supplier ?? (_supplier = new SteamSupplier(capacity, capacity, flowRate));

        public int CurrentAvailableSteam => _supplier.CurrentAvailableSteam;

        public int TryExtractSteam(int requestedAmount = 1) => _supplier.TryExtractSteam(requestedAmount);
        
        
        public override void _Ready()
        {
            _supplier = new SteamSupplier(capacity, capacity, flowRate);
            _supplier.RegisterUpdate();
            _supplier.RegisterLate();
            GasSimulation.Suppliers.Add(_supplier);
        }
        
    }
}
