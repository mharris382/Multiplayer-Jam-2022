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


        
        private IEnumerable<Vector2Int> ProcessCells()
        {
            while (_cells.Count > 0)
            {
                var next = _cells.ExtractMin();
                yield return next;

            }
        }


        private CellDataDictionary<int> _prevStateLookup;

        public void UpdateSimulation()
        {
#if TEST
            TestUpdate();
#else   
            _prevStateLookup = new CellDataDictionary<int>();
            foreach (var cellState in _state.GetCellStates())
            {
                _prevStateLookup.AddOrReplace(cellState.Item1, cellState.Item2);
            }
            
            foreach (var source in _io.GetActiveCellSources())
            {
                if (_prevStateLookup.ContainsKey(source.Position))
                {
                    _prevStateLookup[source.Position] += source.Rate;
                    _prevStateLookup[source.Position] = Mathf.Min(_prevStateLookup[source.Position], 16);
                }
                else
                {
                    Debug.Log($"Source Added {source.Rate} to {source.Position}");
                    _prevStateLookup.Add(source.Position, source.Rate);
                }
            }

            foreach (var activeCellSink in _io.GetActiveCellSinks())
            {
                if (_prevStateLookup.ContainsKey(activeCellSink.GasPosition))
                {
                    _prevStateLookup[activeCellSink.GasPosition] -= activeCellSink.Rate;
                    _prevStateLookup[activeCellSink.GasPosition] =
                        Mathf.Max(_prevStateLookup[activeCellSink.GasPosition], 0);
                }
            }
            UpdateStateFromGrid(GetGasCells());
            //UpdateGridFromState(GetGasCells());
            //
            //AdvanceVelocityField();
            //
            //UpdateStateFromGrid(GetCellsToUpdate(_gridGraph.GetNewPressureStates()));
            
            
            IEnumerable<(Vector2Int cell, int gas)> GetGasCells()
            {
                foreach (var kvp in _prevStateLookup)
                {
                    yield return (kvp.Key, kvp.Value);
                }
            }
#endif
        }

        private void UpdateStateFromGrid(IEnumerable<(Vector2Int, int)> getCellsToUpdate)
        {
            foreach (var valueTuple in getCellsToUpdate)
            {
                var cell = valueTuple.Item1;
                var pressure = valueTuple.Item2;
                _state.SetCell(cell, pressure);
                Debug.Log($"Updated cell state: {cell}={pressure}" );
            }
        }

        private IEnumerable<(Vector2Int, int)> GetCellsToUpdate(IEnumerable<(Vector2Int, int)> cellPressures)
        {
            foreach (var newPressureState in cellPressures )
            {
                var cell = newPressureState.Item1;
                var gas = newPressureState.Item2;
                //was added or was changed
                if (!_prevStateLookup.ContainsKey(cell) || (_prevStateLookup[cell] != gas))
                {
                    _prevStateLookup.Remove(cell);
                    yield return newPressureState;
                }
            }

            if (_prevStateLookup.Count > 0)
            {
                //if the cell is still in the collection, it means the cell just became empty 
                foreach (var kvp in _prevStateLookup)
                {
                    yield return (kvp.Key, 0);
                }
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
                if (!_prevStateLookup.ContainsKey(cell) || (_prevStateLookup[cell] != pressure))
                    _state.SetCell(cell, pressure);
            }

        }
        

        private void AdvanceVelocityField()
        {
            ApplyConvection();
            ApplyExternalForces();
            CalculatePressureToSatisfy();
            ApplyPressure();
            ExtrapolateFluidVelocitiesIntoBufferZone();
            SetSolidCellVelocities();
            
            void ApplyConvection()
            {
            
            }

            void ApplyExternalForces()
            {
            
            }

            void CalculatePressureToSatisfy()
            {
            
            }

            void ApplyPressure()
            {
            
            }

            void ExtrapolateFluidVelocitiesIntoBufferZone()
            {
            
            }

            void SetSolidCellVelocities()
            {
            
            }
        }

        


        public void TestUpdate()
        {
            int cnt = 0;
            HashSet<Vector2Int> modifiedCells = new HashSet<Vector2Int>();
            foreach (var processCell in ProcessCells())
            {
                cnt++;
                modifiedCells.Add(processCell);
            }
            
            Debug.Log($"Counted: {cnt}");
            foreach (var modifiedCell in modifiedCells)
            {
               // _state.SetCell(modifiedCell, GetGasValue(modifiedCell));
            }
        }
    }
}