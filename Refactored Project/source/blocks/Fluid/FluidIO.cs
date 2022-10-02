using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.core;

namespace Game.Blocks.Fluid
{
    /// <summary>
    /// handles fluid sinks and fluid sources.  (static sink/sources)
    /// </summary>
    public class FluidIO
    {
        /// <summary>
        /// basically the key is the solid cell which blocks/unblocks static sinks
        /// </summary>
        private CellDataDictionary<List<CellSink>> _blockSinkMap;

        /// <summary>
        /// basically the key is the solid cell which blocks/unblocks static sources
        /// </summary>
        private CellDataDictionary<List<CellSource>> _blockSourceMap;

        private HashSet<Vector2Int> _unblockedSourceCells = new HashSet<Vector2Int>();
        private HashSet<Vector2Int> _unblockedSinksCells = new HashSet<Vector2Int>();

        public FluidIO(IBlockMap blockMap, IEnumerable<CellSink> fixedSinks, IEnumerable<CellSource> fixedSources)
        {

            _blockSinkMap = new CellDataDictionary<List<CellSink>>();
            _blockSourceMap = new CellDataDictionary<List<CellSource>>();
            Dictionary<Vector2Int, bool> blocksChecked = new CellDataDictionary<bool>();

            foreach (var fixedSink in fixedSinks)
            {
                var block = blockMap.ConvertToBlockCell(fixedSink.GasPosition);
                
                if (!_blockSinkMap.ContainsKey(block))
                    _blockSinkMap.Add(block, new List<CellSink>());

                if (!CheckIsBlocked(block) && !_unblockedSinksCells.Contains(block))
                    _unblockedSinksCells.Add(block);

                _blockSinkMap[block].Add(fixedSink);
            }

            foreach (var fixedSource in fixedSources)
            {
                var block = blockMap.ConvertToBlockCell(fixedSource.Position);
                
                if (!_blockSourceMap.ContainsKey(block)) 
                    _blockSourceMap.Add(block, new List<CellSource>());

                if (!CheckIsBlocked(block) && !_unblockedSourceCells.Contains(block))
                    _unblockedSourceCells.Add(block);
                
                _blockSourceMap[block].Add(fixedSource);
            }


            bool CheckIsBlocked(Vector2Int block)
            {
                if (!blocksChecked.ContainsKey(block))
                {
                    blocksChecked.Add(block, blockMap.IsCellBlocked(block));
                }

                return blocksChecked[block];
            }

#if DEBUG
            Debug.AssertNotEmpty(_blockSinkMap, "No Sinks on map");
            Debug.AssertNotEmpty(_blockSourceMap, "No Sources on map");
            Debug.Log($"# of Unblocked Sinks: {_unblockedSinksCells.Count}");
            Debug.Log($"# of Unblocked Sources: {_unblockedSourceCells.Count}");
#endif
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