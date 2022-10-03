using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Game.Blocks.Gas;
using Game.Blocks.Solids;
using Game.core;
using Godot;
using Debug = Game.core.Debug;

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

        private Stopwatch _gridFromFluidStopwatch = new Stopwatch();
        private Stopwatch _fluidFromGridStopwatch = new Stopwatch();
        private Stopwatch _postStepGridStopwatch = new Stopwatch();
        private Stopwatch _preStepGridStopwatch = new Stopwatch();
        private long _gffMs;
        private long _ffgMs;
        private long _postStepMs;
        private long _preStepMs;

        private int cellCount = 0;
        private const int RECORD_COUNT = 10;
        private (long, long, long, long, int)[] msRecord = new (long, long, long, long, int)[RECORD_COUNT];
        private long _maxFFG, _maxGFF, _maxPost, _maxPre;
        
        public void UpdateSimulation()
        {
            _stepCount++;
            
            _preStepGridStopwatch.Restart();
            PreSimulationStep?.Invoke(_stepCount);
            _preStepGridStopwatch.Stop();
            _preStepMs = _preStepGridStopwatch.ElapsedMilliseconds;
            if (_preStepMs > _maxPre) _maxPre = _preStepMs;
            
            
            _gridFromFluidStopwatch.Restart();
            FluidToGrid();
            _gridFromFluidStopwatch.Stop();
            _gffMs = _gridFromFluidStopwatch.ElapsedMilliseconds;
            if (_gffMs > _maxGFF) _maxGFF = _gffMs;
            
            
            CalculationStep?.Invoke(_stepCount);
            SimulationTimeStep?.Invoke(_stepCount);
            
            cellCount = _simCells.Count;
            
            _fluidFromGridStopwatch.Restart();
            GridToFluid();
            _fluidFromGridStopwatch.Stop();
            _ffgMs = _fluidFromGridStopwatch.ElapsedMilliseconds;
            if (_ffgMs > _maxFFG) _maxFFG = _ffgMs;
            
            
            
            
            _postStepGridStopwatch.Restart();
            PostSimulationStep?.Invoke(_stepCount);
            _postStepGridStopwatch.Stop();
            _postStepMs = _postStepGridStopwatch.ElapsedMilliseconds;
            if (_postStepMs > _maxPost) _maxPost = _postStepMs;


            RecordTimings();
        }


        void RecordTimings()
        {
            int head = _stepCount % RECORD_COUNT;
            msRecord[head] = (_gffMs, _ffgMs, _postStepMs, _preStepMs, _simCells.Count);
            Debug.Log($"\nCell Count = {_simCells.Count}\n \tGrid from Fluid (Fluid>Grid):\t {_gffMs} (Max:{_maxGFF})\n \tFluid from Grid (Grid>Fluid): \t{_ffgMs} (Max:{_maxFFG}\n \tPost Simulation Step:\t {_postStepMs}(Max:{_maxPost})\n \tPre Simulation Step:\t {_preStepMs}(Max:{_maxPre})\n");
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

        
        void GridToFluid()
        {
            _fluid.WriteCells(_simCells.Values.Select(t => (t.Position, t.Gas)), CellWriteMode.OVERWRITE);
        }

        void FluidToGrid()
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