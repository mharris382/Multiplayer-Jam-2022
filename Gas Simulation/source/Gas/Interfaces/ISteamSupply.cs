namespace GasSimulation.Gas
{
    public interface ISteamSupply : ISteamUpdatable
    {
        /// <summary>
        /// how much steam can be extracted on this current steam update
        /// </summary>
        int CurrentAvailableSteam { get; }

        /// <summary>
        /// used to extract steam from a supplier
        /// </summary>
        /// <param name="requestedAmount">desired amount of steam to extract, NOTE: this may or may not be the actual amount received</param>
        /// <returns>the actual amount of steam which was taken from this supplier</returns>
        int TryExtractSteam(int requestedAmount = 1);
    }
}