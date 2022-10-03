using System;
using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.Blocks.Gas.Tilemaps;
using Game.Blocks.Solids;
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
        private readonly FluidIO.ISinks _sinks;
        private readonly FluidIO.ISources _sources;

        public FluidSimulationBuilder(
            GasTilemap gasTilemap,
            SolidBlockTilemap solidBlockTilemap,
            GasSourceTilemap sourceTilemap,
            SinkTileMap sinkTileMap)
        {
            _sources = sourceTilemap;
            _sinks = sinkTileMap;
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
            
            _fluidIo = new FluidIO(gasTilemap, blockMap, sourceTilemap, sinkTileMap);
        }
        
        
        struct GasToBlockConverterConverter : IGasToBlockConverter
        {
            public readonly int GasCellsPerBlockCell;

            public int GasToBlock => GasCellsPerBlockCell;

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

        public FluidSimulation BuildSimulation()
        {
            List<(Vector2Int, int, Vector2Int)> initialState = new List<(Vector2Int, int, Vector2Int)>();
            for (int i = 0; i < 16; i++)
            {
                var cells = _gasTilemap.GetUsedCellsById(i);
                foreach (var cell in cells)
                {
                    initialState.Add((cell,i+1, Vector2Int.U));
                }
            }
            
            FluidTests.RunAllFluidTests(null);
            
            var vectorField = new VectorField2();
            IBlockMap blockMap = new BlockMap(_gridConverter, _solidBlockTilemap);
            IFluid fluidInterface = _gasTilemap;
            
            var simulation = new FluidSimulation(
                fluidInterface, 
                _fluid,
                _fluidIo,
                initialState, 
                _gridConverter,
                blockMap, 
                vectorField);
            
            _gasTilemap.InjectBlockMap(blockMap);
            _gasTilemap.SyncToSim(simulation);
            
            var solidGrid = new DynamicSolidGrid(
                simulation,
                _solidBlockTilemap,
                _gridConverter,
                fluidInterface);
            
            _fluidIo.ConnectToSim(simulation);
            
            return simulation;
        }
        
     
    }
}