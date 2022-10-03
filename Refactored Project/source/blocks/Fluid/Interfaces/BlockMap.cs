using System;
using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.Blocks.Gas.Tilemaps;

namespace Game.Blocks.Fluid
{
    public class BlockMap : IBlockMap
    {
        
        private IGasToBlockConverter _converter;
        private HashSet<Vector2Int> _blockedCells;
        private SolidBlockTilemap _solidBlockTilemap;
        

        public BlockMap(IGasToBlockConverter converter, SolidBlockTilemap solidBlockTilemap)
        {
            _converter = converter;
            _solidBlockTilemap = solidBlockTilemap;
            _blockedCells = new HashSet<Vector2Int>();
            foreach (var cell in solidBlockTilemap.GetAllBlockedCells())
            {
                _blockedCells.Add(new Vector2Int(cell));
            }
            solidBlockTilemap.OnBlockCellAdded += (cell, tileId) => OnBlockAdded(cell);
            solidBlockTilemap.OnBlockCellRemoved += (cell, tileId) => OnBlockRemoved(cell);
        }

        public Vector2Int ConvertToBlockCell(Vector2Int gasCell)
        {
            return _converter.GasToBlockCell(gasCell);
        }

        public bool IsCellBlocked(Vector2Int blockCell)
        {
            return _blockedCells.Contains(blockCell);
        }
        
        private void OnBlockRemoved(Vector2Int blockCell)
        {
            _blockedCells.Remove(blockCell);
            BlockCellRemoved?.Invoke(blockCell);
        }

        private void OnBlockAdded(Vector2Int blockCell)
        {
            _blockedCells.Add(blockCell);
            BlockCellAdded?.Invoke(blockCell);
        }
        
        public event Action<Vector2Int> BlockCellAdded;
        public event Action<Vector2Int> BlockCellRemoved;
        
        
        
    }
}