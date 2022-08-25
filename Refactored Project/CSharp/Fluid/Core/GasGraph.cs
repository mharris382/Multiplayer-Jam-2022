namespace Game.Fluid.Core
{
    /// <summary>
    /// This class should be responsible for storing the current gas state.  It should not track gas changes, only the CURRENT state.  Force systems will use this graph to find changes.
    /// </summary>
    public abstract class GasGraph
    {
        /// <summary>
        /// returns the amount of gas in this cell
        /// </summary>
        /// <param name="cell">cell location in tilemap space<see cref="Godot.TileMap.WorldToMap"/></param>
        /// <returns></returns>
        public abstract int GetGas(Cell cell);
        
        /// <summary>
        /// attempts to modify the amount of gas in the cell.  This function should only be used externally for sources/sinks.  For forces use   
        /// </summary>
        /// <param name="cell">cell location in tilemap space <see cref="Godot.TileMap.WorldToMap"/></param>
        /// <param name="amount">the amount requested may not match the amount added, which is why a ref parameter is used here <para>
        ///For example: if the current gas = 14 and we try to add 4 gas we would exceed the maximum capacity (16).  The actual amount of gas that changed would be 2.  There may be other reasons the full amount was not added.  One case may be that the fluid
        /// has a flow capacity, and only allows a certain amount of change per iteration.  If that amount has already been met we would not be able to modify the gas further on this frame, even if there is still enough room 
        /// </para> </param>
        /// <returns>true if the amount of gas in cell was changed AT ALL. False only if the amount of gas in the cell was not modified AT ALL.  </returns>
        public abstract bool TryModifyGas(Cell cell, ref int amount);

        
        /// <summary>
        /// attempts to move the specified amount of gas from one cell to the other, while abiding by the conservation of mass.  The total amount of gas in the graph before should exactly equal the amount after this function is called 
        /// </summary>
        /// <param name="from">cell location to remove gas from in tilemap space <see cref="Godot.TileMap.WorldToMap"/></param>
        /// <param name="to">cell location to add gas to in tilemap space <see cref="Godot.TileMap.WorldToMap"/></param>
        /// <param name="amountToTransfer"></param>
        public abstract void TransferGas(Cell from, Cell to, ref int amountToTransfer);
        
        
    }
}