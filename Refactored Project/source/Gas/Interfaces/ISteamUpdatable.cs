namespace Game.Gas
{
    /// <summary>
    /// interface used to register callbacks BEFORE each steam update
    /// </summary>
    public interface ISteamUpdatable
    {
        /// <summary>
        /// called once before the each steam fluid simulation iteration 
        /// </summary>
        void SteamUpdate();
    }
    
    /// <summary>
    /// interface used to register callbacks AFTER each steam update
    /// </summary>
    public interface ISteamLateUpdatable
    {
        /// <summary>
        ///  called once after each steam fluid simulation iteration 
        /// </summary>
        void SteamLateUpdate();
    }

}