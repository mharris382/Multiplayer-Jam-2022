using System;
using System.Collections.Generic;
using System.Linq;
using Game.Blocks.Gas;
using Game.core;
using Godot;

namespace Game.Blocks.Fluid
{
    
    public class FluidSimulation
    {
        private readonly IGasToBlockConverter _converter;
        private readonly IBlockMap _blockMap;
        private readonly IFluidState _state;
        private readonly FluidIO _io;

        private PriorityQueue<Vector2Int> _cells;


        private GridGraph _gridGraph;

        /// <summary>
        /// key is the buffer cell, value is neighbor that has gas.  note their can be more than one neighbor, but we only need to store one.
        /// If that neighbor is removed, we should check for other non-empty neighbors, if none are found we should remove the cell from the buffer area.
        /// </summary>
        private CellDataDictionary<Vector2Int> _bufferArea;

        public FluidSimulation(IFluidState state, FluidIO io,
            IEnumerable<(Vector2Int cell, int density, Vector2Int velocity)> initialState, 
            IGasToBlockConverter converter, IBlockMap blockMap)
        {
            _state = state;
            this._io = io;
            _converter = converter;
            _blockMap = blockMap;
            _cells = new PriorityQueue<Vector2Int>(100000);


            var s = new CellDataDictionary<int>();
            _bufferArea = new CellDataDictionary<Vector2Int>();
            foreach (var gas in initialState)
            {
                s.AddOrChange(gas.cell, gas.density);
            }

            _gridGraph = new GridGraph(
                c => _blockMap.IsCellBlocked(_blockMap.ConvertToBlockCell(c)),
                s, GraphType.DIRECTED_WEIGHTED);
        }
        
        
        
        
        private CellDataDictionary<int> _gasStates;
        private Graph<Vector2Int> _gasGraph;
        public void UpdateSimulation()
        {
            _gasGraph = _gasGraph ?? new Graph<Vector2Int>(GraphType.UNDIRECTED_UNWEIGHTED);
            _gasStates = _gasStates ?? new CellDataDictionary<int>();
            IEnumerable<(Vector2Int cell, int gas)> GetGasCells()
            {
                foreach (var kvp in _gasStates)
                {
                    yield return (kvp.Key, !IsCellBlocked(kvp.Key) ? kvp.Value : 0);
                }
            }
            
            var cellStates = _state.GetCellStates().ToList();
            foreach (var cellState in cellStates)
            {
                var cell = cellState.Item1;
                var pressure = cellState.Item2;
                
                _gasGraph.AddVertex(cell);
                _gasStates.AddOrReplace(cell, pressure);
            }
            
            UpdateGridFromState(GetGasCells());
            UpdateSourceSinks();
            UpdateStateFromGrid(GetGasCells());

            #region [Save for later]

            //all non-empty nodes are now on the graph, need to build buffer zone around non-empty nodes
            // foreach (var cellState in cellStates)
            // {
            //     var cell = cellState.Item1;
            //     var pressure = cellState.Item2;
            //     void AddNeighbor(Vector2Int offset)
            //     {
            //         var neighbor = cell + offset;
            //         if (!IsNeighborBlocked(cell, offset))
            //         {
            //             
            //         }
            //     }
            // }

            //bool IsNeighborBlocked(Vector2Int cell, Vector2Int offset)
            //{
            //    var modX = cell.x % gasPerBlock;
            //    var modY = cell.y % gasPerBlock;
            //    if ((modX != 0 && modY != 0) || (modX != (gasPerBlock - 1) && modY != (gasPerBlock-1)))
            //    {
            //        return false;
            //    }
            //    var neighbor = cell + offset;
            //    return IsCellBlocked(neighbor);
            //}

            #endregion
            
            
            bool IsCellBlocked(Vector2Int cell) => _blockMap.IsCellBlocked(_converter.GasToBlockCell(cell));
            
        }

        
        private void UpdateSourceSinks()
        {
            foreach (var source in _io.GetActiveCellSources().Where(t => !_blockMap.IsCellBlocked(t.Position)))
            {
                if (_gasStates.ContainsKey(source.Position))
                {
                    _gasStates[source.Position] += source.Rate;
                    _gasStates[source.Position] = Mathf.Min(_gasStates[source.Position], 16);
                }
                else
                {
                    //Debug.Log($"Source Added {source.Rate} to {source.Position}");
                    _gasStates.Add(source.Position, source.Rate);
                }
            }

            
            foreach (var activeCellSink in _io.GetActiveCellSinks().Where(t => !_blockMap.IsCellBlocked(t.GasPosition)))
            {
                if (_gasStates.ContainsKey(activeCellSink.GasPosition))
                {
                    _gasStates[activeCellSink.GasPosition] -= activeCellSink.Rate;
                    _gasStates[activeCellSink.GasPosition] =
                        Mathf.Max(_gasStates[activeCellSink.GasPosition], 0);
                }
            }
        }

        private void UpdateStateFromGrid(IEnumerable<(Vector2Int, int)> getCellsToUpdate)
        {
            foreach (var valueTuple in getCellsToUpdate)
            {
                var cell = valueTuple.Item1;
                var pressure = _blockMap.IsCellBlocked(cell) ? 0 : valueTuple.Item2;
                _state.SetCell(cell, pressure);
                //Debug.Log($"Updated cell state: {cell}={pressure}" );
            }
        }
        
       
        public void UpdateGridFromState(IEnumerable<(Vector2Int cell, int gas)> cells)
        {
            _gridGraph.StartUpdate(cells);


            var newStates = _gridGraph.GetNewPressureStates();
            foreach (var valueTuple in newStates)
            {
                var cell = valueTuple.Item1;
                var pressure = valueTuple.Item2;
                //TODO: try remove
                if (!_gasStates.ContainsKey(cell) || (_gasStates[cell] != pressure))
                    _state.SetCell(cell, pressure);
            }

        }
        

    }
}