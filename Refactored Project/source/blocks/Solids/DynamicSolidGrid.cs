using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Game.Blocks.Fluid;
using Game.Blocks.Gas;
using Game.Blocks.Gas.Tilemaps;
using Cell = Game.Blocks.Gas.Vector2Int;
using Debug = Game.core.Debug;

namespace Game.Blocks.Solids
{
    public interface ISimulationUpdater
    {
        event Action<int> PreSimulationStep;
        event Action<int> CalculationStep;
        event Action<int> SimulationTimeStep;
        event Action<int> PostSimulationStep;
    }
    public class DynamicSolidGrid
    {
        private readonly ISimulationUpdater _updater;
        private readonly SolidBlockTilemap _solidBlockTilemap;
        private readonly IGasToBlockConverter _converter;
        private readonly IFluid _fluid;

        private HashSet<Cell> _justUnblocked = new HashSet<Cell>();
        private HashSet<Cell> _dirtyBlocks = new HashSet<Cell>();
        
        private Stopwatch _watch;
        private bool gasGridDirty = false;
        
        public DynamicSolidGrid(ISimulationUpdater updater,
            SolidBlockTilemap solidBlockTilemap, 
            IGasToBlockConverter converter,
            IFluid fluid)
        {
            _updater = updater;
            _solidBlockTilemap = solidBlockTilemap;
            _converter = converter;
            _fluid = fluid;

            //_updater.PreSimulationStep += UpdaterOnPreSimulationStep;
            //_updater.CalculationStep += UpdaterOnCalculationStep;
            //_updater.SimulationTimeStep += UpdaterOnSimulationTimeStep;
            //_updater.PostSimulationStep += UpdaterOnPostSimulationStep;

            updater.PostSimulationStep += (num) =>
            {
                if (gasGridDirty)
                {
                    gasGridDirty = false;
                    _fluid.WriteCells(_dirtyBlocks.Select(t=> (t, 0)), CellWriteMode.OVERWRITE);
                    _dirtyBlocks.Clear();
                }
            };
            _solidBlockTilemap.OnBlockCellAdded += (cell, i1) =>
            {
                _justUnblocked.Remove(cell);
                gasGridDirty = true;
                if (!_dirtyBlocks.Contains(cell))
                {
                    _dirtyBlocks.Add(cell);
                    //Debug.Log($"Dynamic Solid Grid marked as dirty {cell}");
                }
            };
            
            _solidBlockTilemap.OnBlockCellRemoved += (cell, i1) =>
            {
                _dirtyBlocks.Remove(cell);
                if (!_justUnblocked.Contains(cell))
                {
                    //Debug.Log($"Dynamic Solid Grid marked as dirty {cell}");
                    _justUnblocked.Add(cell);
                }
            };
        }

        private const int logEveryNSteps = 5;
        
        private void UpdaterOnSimulationTimeStep(int stepNumber)
        {
            if (stepNumber % logEveryNSteps == 0)
            {
                Debug.Log("Simulation Step");
            }
        }

        private void UpdaterOnCalculationStep(int stepNumber)
        {
            if (stepNumber % logEveryNSteps == 0)
            {
                Debug.Log("Calculation Step");
            }
        }

        
        private void UpdaterOnPostSimulationStep(int stepNumber)
        {
            _watch.Stop();
            if (stepNumber % logEveryNSteps == 0)
            {
                Debug.Log($"Post Simulation Step: took {_watch.ElapsedMilliseconds} MS");
            }
        }

        private void UpdaterOnPreSimulationStep(int stepNumber)
        {
            _watch = new Stopwatch();
            _watch.Start();
            if (stepNumber % logEveryNSteps == 0)
            {
                Debug.Log("Pre Simulation Step");
            }
        }


        // class SolidGridCell
        // {
        //     private readonly DynamicSolidGrid _dynamicSolidGrid;
        //     
        //     private readonly Vector2Int _gasCell;
        //     private readonly Vector2Int _blockCell;
        //
        //     private Vector2Int[,] _localCells;
        //     
        //     public bool IsValid
        //     {
        //         get;
        //         set;
        //     }
        //     
        //
        //     public SolidGridCell(DynamicSolidGrid dynamicSolidGrid, Cell gasCell )
        //     {
        //         _blockCell = dynamicSolidGrid._converter.GasToBlockCell(gasCell);
        //         _dynamicSolidGrid = dynamicSolidGrid;
        //         _gasCell = gasCell;
        //         
        //         
        //         int gasCellsPerBlockCell = dynamicSolidGrid._converter.GasToBlock;
        //         _localCells = new Vector2Int[gasCellsPerBlockCell, gasCellsPerBlockCell];
        //         
        //         IsValid = true;
        //     }
        // }
        //
        // struct GasCell
        // {
        //     private readonly SolidGridCell _containedSolidCell;
        //     private readonly Vector2Int _localOffset;
        //
        //
        //     public GasCell(SolidGridCell containedSolidCell, Cell localOffset)
        //     {
        //         _containedSolidCell = containedSolidCell;
        //         _localOffset = localOffset;
        //     }
        // }
    }
}