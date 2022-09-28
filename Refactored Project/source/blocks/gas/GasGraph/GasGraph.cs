using System;
using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.core;
using Godot;

namespace Game.blocks.gas.GasGraph
{
    public class GasGraph
    {
        private readonly int _maxSize;
        
        private Graph<Vector2Int> _gasCellGraph;
        private Graph<Vector2Int> _blockToCellGraph;
        private Dictionary<Vector2Int, CellData> _cellData;
        
        
        /// <summary> for a gas graph to be valid, it needs to have at least one non-empty vertex </summary>
        private bool _isValid;

        public GasGraph(int maxSize = 10000)
        {
            this._maxSize = maxSize;
            _gasCellGraph = new Graph<Vector2Int>(GraphType.DIRECTED_WEIGHTED);
            
            _cellData = new Dictionary<Vector2Int, CellData>(_maxSize);
        }


        public struct CellData
        {
            public int gasAmount;
        }
    }

    public class BlockToGasMap
    {
        private Dictionary<Vector2Int, Vector2Int> _blockToGasTopLeft;
        private Dictionary<Vector2Int, Vector2Int> _gasToBlock;

        public IEnumerable<Vector2Int> GetAllGasCellsInBlockCell(Vector2Int blockCell)
        {
            throw new NotImplementedException();
        }
    }
    
    
}