using Godot;

namespace Game.Gas
{
    /// <summary>
    /// godot node wrapper for steam supplier
    /// <seealso cref="SteamSupplier"/>
    /// </summary>
    public class SteamSupplierNode : Node2D ,ISteamUpdatable
    {
        private int capacity = 16;
        private int flowRate = 3;
        
        
        private SteamSupplier _supplier;
        
        private SteamSupplier Supplier => _supplier ?? (_supplier = new SteamSupplier(capacity, capacity, flowRate));

        public int CurrentAvailableSteam => _supplier.CurrentAvailableSteam;

        public int TryExtractSteam(int requestedAmount = 1) => _supplier.TryExtractSteam(requestedAmount);
        
        
        public override void _Ready()
        {
            this.RegisterUpdate();
            
            _supplier = new SteamSupplier(capacity, capacity, flowRate);
            _supplier.RegisterUpdate();
            _supplier.RegisterLate();
            
            if(GasSimulation.Suppliers.ContainsKey(_supplier)==false)
                GasSimulation.Suppliers.Add(_supplier, Vector2.Zero);
            
            GasSimulation.Suppliers[_supplier] = GlobalPosition;
        }

        public void SteamUpdate()
        {
            GasSimulation.Suppliers[_supplier] = GlobalPosition;
        }
    }
}
