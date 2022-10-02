using System;
using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.Blocks.Gas.Tilemaps;
using Game.core;
using Godot;

namespace Game.Blocks.Fluid
{
    public class FluidSimulationBuilder
    {
        private readonly IGasToBlockConverter _gridConverter;
        private readonly FluidIO _fluidIo;
        private readonly  IFluidState _fluid;
        private readonly GasTilemap _gasTilemap;
        private readonly SolidBlockTilemap _solidBlockTilemap;

        public FluidSimulationBuilder(
            GasTilemap gasTilemap,
            SolidBlockTilemap solidBlockTilemap,
            GasSourceTilemap sourceTilemap,
            SinkTileMap sinkTileMap)
        {
            int gasCellSize = (int)gasTilemap.CellSize.x;
            int blockCellSize = (int)solidBlockTilemap.CellSize.x;
            _solidBlockTilemap = solidBlockTilemap;
            int gasCellsPerBlock = blockCellSize/gasCellSize;
            if(!Debug.Assert(gasCellsPerBlock > 0, "ERROR! WTF HAPPENED HERE"))
            {
                throw new Exception();
            }
            _gridConverter = new GasToBlockConverterConverter(gasCellsPerBlock);
            this._gasTilemap = gasTilemap;
            
            IBlockMap blockMap = new BlockMap(_gridConverter, solidBlockTilemap);
            IEnumerable<CellSink> fixedSinks = sinkTileMap.GetSinkCells();
            IEnumerable<CellSource> fixedSources = sourceTilemap.GetSourceCells();
            
            _fluidIo = new FluidIO(blockMap, fixedSinks, fixedSources);
            _fluid = new FluidState(gasTilemap);
        }
        
        
        struct GasToBlockConverterConverter : IGasToBlockConverter
        {
            public readonly int GasCellsPerBlockCell;

            public GasToBlockConverterConverter(int gasCellsPerBlockCell)
            {
                GasCellsPerBlockCell = gasCellsPerBlockCell;
            }

            public Vector2Int GasToBlockCell(Vector2Int gasCell)
            {
                return new Vector2Int(gasCell.x / GasCellsPerBlockCell, gasCell.y / GasCellsPerBlockCell);
            }

            public IEnumerable<Vector2Int> BlockToGasCells(Vector2Int blockCell)
            {
                var start = new Vector2Int(blockCell.x * GasCellsPerBlockCell, blockCell.y * GasCellsPerBlockCell);
                for (int i = 0; i < GasCellsPerBlockCell; i++)
                {
                    for (int j = 0; j < GasCellsPerBlockCell; j++)
                    {
                        yield return start + new Vector2Int(i, j);
                    }
                }
            }
        }

        struct FluidState : IFluidState
        {
            private readonly GasTilemap _gasTilemap;

            public FluidState(GasTilemap gasTilemap)
            {
                this._gasTilemap = gasTilemap;
            }

            public void SetCell(Vector2Int cell, int gasValue)
            {
                _gasTilemap.SetSteam(new Vector2(cell.x, cell.y),  gasValue);
            }

            public IEnumerable<(Vector2Int, int)> GetCellStates()
            {
                foreach (var gasCells in _gasTilemap.GetGasCells())
                {
                    yield return (gasCells.cell, gasCells.gas);
                }
            }
        }
        public FluidSimulation BuildSimulation()
        {
            List<(Vector2Int, int, Vector2Int)> initialState = new List<(Vector2Int, int, Vector2Int)>();
            for (int i = 0; i < 16; i++)
            {
                var cells = _gasTilemap.GetUsedCellsById(i);
                foreach (var cell in cells)
                {
                    initialState.Add((cell,i+1, Vector2Int.Up));
                }
            }
            
            return new FluidSimulation(_fluid, _fluidIo, initialState, _gridConverter, new BlockMap(_gridConverter, _solidBlockTilemap));
        }
    }
}