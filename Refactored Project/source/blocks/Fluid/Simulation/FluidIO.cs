using System;
using System.Collections.Generic;
using System.Linq;
using Game.Blocks.Gas;
using Game.Blocks.Solids;
using Game.core;

namespace Game.Blocks.Fluid
{
    /// <summary>
    /// handles fluid sinks and fluid sources.  (static sink/sources)
    /// </summary>
    public class FluidIO
    {
        public interface ISinks
        {
            IEnumerable<CellSink> GetActiveSinks(Predicate<Vector2Int> condition);
        }
        public interface ISources
        {
            IEnumerable<CellSource> GetActiveSources(Predicate<Vector2Int> condition);
        }
        /// <summary>
        /// basically the key is the solid cell which blocks/unblocks static sinks
        /// </summary>
        [Obsolete]private CellDataDictionary<List<CellSink>> _blockSinkMap;

        /// <summary>
        /// basically the key is the solid cell which blocks/unblocks static sources
        /// </summary>
        [Obsolete]private CellDataDictionary<List<CellSource>> _blockSourceMap;

        [Obsolete]private HashSet<Vector2Int> _unblockedSourceCells = new HashSet<Vector2Int>();
        
        [Obsolete]private HashSet<Vector2Int> _unblockedSinksCells = new HashSet<Vector2Int>();
        private readonly ISinks _sinks;
        private readonly IFluid _fluid;
        private readonly ISources _sources;
        private readonly IBlockMap _blockMap;


        
        public FluidIO( IFluid fluid, IBlockMap blockMap, ISources sources, ISinks sinks)
        {
            _fluid = fluid;
            this._sources = sources;
            this._sinks = sinks;
            this._blockMap = blockMap;
            
            

#if DEBUG
            Debug.AssertNotEmpty(_blockSinkMap, "No Sinks on map");
            Debug.AssertNotEmpty(_blockSourceMap, "No Sources on map");
            Debug.Log($"# of Unblocked Sinks: {_unblockedSinksCells.Count}");
            Debug.Log($"# of Unblocked Sources: {_unblockedSourceCells.Count}");
#endif
        }

        public void ConnectToSim(ISimulationUpdater updater)
        {
            updater.PostSimulationStep += UpdaterOnPostSimulationStep;
        }

        private void UpdaterOnPostSimulationStep(int obj)
        {
            Predicate<Vector2Int> cond = cell => !_blockMap.IsCellBlocked(cell);
            
            var valueTuples = _sources.GetActiveSources(cond).Select(t => (t.Position, t.Rate))
                ///.Union(_sinks.GetActiveSinks(cond).Select(t => (t.GasPosition, -t.Rate)))
                ;
            if (obj % 10 == 0)
            {
                var srcs = _sources.GetActiveSources(cond).ToList();
                var snks = _sinks.GetActiveSinks(cond).ToList();
                var srcSum = srcs.Sum(t => t.Rate);
                var snkSum = snks.Sum(t => t.Rate);
                
                Debug.Log($"# Sources = {srcs.Count} (total={srcSum})\n# Sinks = {snks.Count} (total={snkSum})");
            }
            
            _fluid.WriteCells(valueTuples.ToList(), CellWriteMode.BLEND_ADD);
        }

        public IEnumerable<CellSource> GetActiveCellSources()
        {
            foreach (var unblockedSourceCell in _unblockedSourceCells)
            {
                foreach (var cellSource in _blockSourceMap[unblockedSourceCell])
                {
                    yield return cellSource;
                }
            }
        }

        public IEnumerable<CellSink> GetActiveCellSinks()
        {
            foreach (var unblockedSinkCell in _unblockedSinksCells)
            {
                foreach (var cellSink in _blockSinkMap[unblockedSinkCell])
                {
                    yield return cellSink;
                }
            }
        }
        public IEnumerable<(Vector2Int, IEnumerable<CellSink>)> GetBlockCellsWithSinks()
        {
            foreach (var unblockedSinkCell in _unblockedSinksCells)
            {
                yield return (unblockedSinkCell, _blockSinkMap[unblockedSinkCell]);
            }
        }
        public IEnumerable<(Vector2Int, IEnumerable<CellSource>)> GetBlockCellsWithSources()
        {
            foreach (var unblockedSourceCell in _unblockedSourceCells)
            {
                yield return (unblockedSourceCell, _blockSourceMap[unblockedSourceCell]);
            }
        }
    }
}