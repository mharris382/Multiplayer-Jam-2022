using System;
using System.Collections.Generic;
using System.Linq;
using Game.Blocks.Gas;
using Game.Blocks.Solids;
using Game.core;
using Godot;

namespace Game.Blocks.Fluid
{

    public class FluidSimulation : ISimulationUpdater
    {
        private readonly IGasToBlockConverter _converter;
        private readonly IBlockMap _blockMap;
        private readonly IFluid _fluid;
        private readonly IFluidState _state;
        private readonly FluidIO _io;

        public event Action<int> PreSimulationStep;
        public event Action<int> CalculationStep;
        public event Action<int> SimulationTimeStep;
        public event Action<int> PostSimulationStep;

        private GridGraph _gridGraph;
        private int _stepCount;

        /// <summary>
        /// key is the buffer cell, value is neighbor that has gas.  note their can be more than one neighbor, but we only need to store one.
        /// If that neighbor is removed, we should check for other non-empty neighbors, if none are found we should remove the cell from the buffer area.
        /// </summary>

        private CellDataDictionary<Vector2Int> _bufferArea;

        private CellDataDictionary<int> _gasStates;
        private Graph<Vector2Int> _gasGraph;

        private IVectorField _vectorField;
        private CellDataDictionary<SimulationCell> _simCells;

        public FluidSimulation(
            IFluid fluid,
            IFluidState state,
            FluidIO io,
            IEnumerable<(Vector2Int cell, int density, Vector2Int velocity)> initialState,
            IGasToBlockConverter converter,
            IBlockMap blockMap,
            IVectorField vectorField)
        {
            List<SimulationCell> cells = new List<SimulationCell>();
            _simCells = new CellDataDictionary<SimulationCell>();
            foreach (var valueTuple in initialState)
            {
                _simCells.Add(valueTuple.cell,
                    new SimulationCell(valueTuple.cell, valueTuple.density, valueTuple.cell));
            }

            
            _fluid = fluid;
            _state = state;
            this._io = io;
            _converter = converter;
            _blockMap = blockMap;
            _vectorField = vectorField;
            
            _fluid.OnCellClearedOfGas += i =>
            {
                _simCells.Remove(i);
            };
            _blockMap.BlockCellAdded += i =>
            {
                foreach (var cell in _converter.BlockToGasCells(i))
                {
                    _simCells.Remove(cell);
                }
            };
            _blockMap.BlockCellRemoved += i =>
            {
                foreach (var cell in _converter.BlockToGasCells(i))
                {
                    _simCells.Remove(cell);
                }
            };
        }


        private void UpdateSourceSinks()
        {
            var srcs = from src in _io.GetActiveCellSources()
                where (!_simCells.ContainsKey(src.Position) || (_simCells[src.Position].Gas < 16))
                select new SimulationCell(
                    src.Position, 
                    Mathf.Max(16-_simCells[src.Position].Gas, src.Rate), 
                    Vector2Int.Z);
          
            var snks = from snk in _io.GetActiveCellSinks() 
                where _simCells.ContainsKey(snk.GasPosition) 
                select new SimulationCell(
                    snk.GasPosition,
                    Mathf.Min(_simCells[snk.GasPosition].Gas, snk.Rate),
                    _simCells[snk.GasPosition].Velocity);

            
            foreach (var sinSnk in snks)
            {
                
            }
        }

        public struct SimulationCell
        {
            private readonly Vector2Int _pos;
            private readonly int _gas;
            private readonly int _velocityX;
            private readonly int _velocityY;
            public Vector2Int Position => _pos;
            public Vector2Int Velocity => new Vector2Int(_velocityX, _velocityY);
            public int Gas => _gas;

            public SimulationCell(Vector2Int pos, int gas, Vector2Int velocity)
            {
                _pos = pos;
                _gas = gas;
                _velocityX = velocity.x;
                _velocityY = velocity.y;
            }

            public override int GetHashCode()
            {
                return _pos.GetHashCode();
            }
        }

        public void UpdateSimulation()
        {
            _stepCount++;
            
            Debug.Log("Starting Pre Simulation Step");
            PreSimulationStep?.Invoke(_stepCount);
            Debug.Log("Finished Pre Simulation Step");
            
            UpdateGridFromFluid();
            
            CalculationStep?.Invoke(_stepCount);
            
            SimulationTimeStep?.Invoke(_stepCount);
            
            UpdateFluidFromGrid();
            
            Debug.Log("Starting Post Simulation Step");
            PostSimulationStep?.Invoke(_stepCount);
            Debug.Log("Finished Post Simulation Step");
            
        }


        IEnumerable<SimulationCell> GetGridCells() => _simCells.Values;

        /// <summary>
        /// cooresponds to getting the fluid state from the simulation
        /// </summary>
        /// <returns></returns>
        IEnumerable<SimulationCell> GetFluidCells()
        {
            return  _fluid.GetUseCellStates()
                .Select(t => new SimulationCell(t.Item1, t.Item2, _vectorField.Get(t.Item1)));
            
        }

        
        void UpdateFluidFromGrid()
        {
            var r = 
                from c in _simCells.Values
                select (c.Position, c.Gas);
            _fluid.WriteCells(r, CellWriteMode.OVERWRITE);

        }

        void UpdateGridFromFluid()
        {
            foreach (var simulationCell in GetFluidCells())
            {
                _simCells.AddOrChange(simulationCell.Position, simulationCell);
            }
        }

    private IVectorField GetVectorField()
        {
            return _vectorField;
        }

      
        
        
    }


}