using System.Collections.Generic;
using Godot;

namespace Game.Gas
{
    public static class GasSimulation
    {
        public static event System.Action PreGasIteration;
        public static event System.Action PostGasIteration;

        public static Dictionary<ISteamSupply, Vector2> Suppliers = new Dictionary<ISteamSupply, Vector2>();
        
        public static void RegisterLate(this ISteamLateUpdatable updatable)
        {
            PostGasIteration += updatable.SteamLateUpdate;
        }
        public static void UnregisterLate(this ISteamLateUpdatable updatable)
        {
            PostGasIteration -= updatable.SteamLateUpdate;
        }

        public static void RegisterUpdate(this ISteamUpdatable updatable)
        {
            PreGasIteration += updatable.SteamUpdate;
        }
        public static void UnregisterUpdate(this ISteamUpdatable updatable)
        {
            PreGasIteration -= updatable.SteamUpdate;
        }

        
        

        public static void DoPreUpdate()
        {
            PreGasIteration?.Invoke();
        }
        public static void DoPostUpdate()
        {
            PostGasIteration?.Invoke();
        }
    }
}