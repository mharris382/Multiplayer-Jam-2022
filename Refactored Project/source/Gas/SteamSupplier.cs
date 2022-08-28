using System;
using Godot;

namespace Game.Gas
{
    public class SteamSupplier : ISteamSupply, ISteamLateUpdatable
    {
        private readonly int _steamCapacity;
        private int _maxFlowRate;
        private int _steamSupply;

        private int _amountExtractedThisFrame;
        private float _replenishRate;
        private float _replenishingAmount;

        public int CurrentAvailableSteam
        {
            get
            {
                var amount = _steamSupply;
                var remainingFlow = _maxFlowRate - _amountExtractedThisFrame;
                return amount;
            }
        }

        public Vector2 GetWorldSpacePosition()
        {
            return Vector2.Zero;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="steamSupply">the initial amount of stored steam, by default the capacity is equal to this value.</param>
        /// <param name="steamCapacity"></param>
        /// <param name="maxFlowRate">the maximum amount of gas that can be extracted per steam update</param>
        /// <param name="replenishRate">positive value will try to increase the stored steam supply by this amount each update, by default the internal supply of gas does not replenish</param>
        internal SteamSupplier(int steamSupply, int steamCapacity, int maxFlowRate, float replenishRate=0)
        {
            _steamSupply = steamSupply;
            _steamCapacity = steamCapacity;
            _maxFlowRate = maxFlowRate;
            _replenishRate = replenishRate;
        }
        SteamSupplier(int steamSupply, int maxFlowRate, float replenishRate=0) : this(steamSupply, steamSupply, maxFlowRate, replenishRate)
        {
            _steamSupply = steamSupply;
            _steamCapacity = steamSupply;
            _maxFlowRate = maxFlowRate;
            _replenishRate = replenishRate;
        }
        public void SteamUpdate()
        {
            _amountExtractedThisFrame = 0;//need to reset this each update or the whole thing explodes 
            GD.Print("Updating Steam Supplier");
        }

        private int GetReplenishAmount()
        {
            if (_replenishRate <= 0)
            {
                if (_replenishRate < 0)
                    throw new NotImplementedException("Haven't implemented internally depleting air supply");
                return 0;
            }

            var amountToAdd = 0;
            if (_replenishingAmount > 1)
            {
                amountToAdd = Mathf.FloorToInt(_replenishingAmount);
                _replenishingAmount-= amountToAdd;
            }
            return amountToAdd;
        }
      
        public int TryExtractSteam(int requestedAmount = 1)
        {
            throw new System.NotImplementedException();
        }

        public void SteamLateUpdate()
        {
            var replenishAmount = GetReplenishAmount();
            _steamSupply += replenishAmount;
            _steamSupply = Mathf.Clamp(_steamSupply, 0, _steamCapacity);
        }
    }
}