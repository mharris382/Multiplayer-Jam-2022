﻿using System;
using System.Collections.Generic;
using Game.core;
using Godot;

namespace Game.Blocks.Gas
{
    /// <summary>
    /// tilemap containing the solid blocks that block air and can be walked on.  This is the tilemap that the players
    /// can modify dynamically by building and removing tiles.
    /// <para>
    /// The gas simulation will break if it the solid block map not fully enclosed, therefore the players cannot remove
    /// the boundary blocks from this map.  We determine the boundary by finding a rect containing all the blocks.  The
    /// boundary is setup implicitly by the tilemap's initial state.
    /// </para>
    /// </summary>
    public class SolidBlockTilemap : TileMap
    {
        private Vector2 _min, _max;
        [Export()]
        private string blockTileName = "";
        [Export()]
        private bool manualBounds = true;
        [Export()]
        private Vector2 Max = new Vector2(25,25);
        [Export()]
        private Vector2 Min = new Vector2(-1, -1);
        private int _defaultBlockID = 0;
        
        public override void _Ready()
        {
            GasStuff.BlockTilemap = this;
            
            var usedCells = GasStuff.BlockTilemap.GetUsedCells();
            if (manualBounds)
            {
                _max = Max;
                _min = Min;
            }
            else
            {
                _min = new Vector2(float.MaxValue, float.MaxValue);
                _max = new Vector2(float.MinValue, float.MinValue);
                foreach (var usedCell in usedCells)
                {
                    var v = (Vector2)usedCell;
                    if (v.x > _max.x) _max.x = v.x;
                    else if (v.x < _min.x) _min.x = v.x;
                    if (v.y < _min.y) _min.y = v.y;
                    else if (v.y > _max.y) _max.y = v.y;
                }
            }
            

            if ((_defaultBlockID = TileSet.FindTileByName(blockTileName)) == -1)
            {
                _defaultBlockID = 1;
                Debug.LogWarning($"No block named {blockTileName} found in {TileSet.ResourceName}");
            }
            Debug.Log($"Min = {_min}, Max = {_max}");
        }

        public override void _Process(float delta)
        {
            if (GasStuff.BlockTilemap == null) GasStuff.BlockTilemap = this;
            base._Process(delta);
        }

        /// <summary>
        /// returns true if the cell contains a solid block or not
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCellSolid(Vector2 cell)
        {
            return GetCellv(cell) != -1;
        }

        /// <summary>
        /// builds the default solid block at the given cell.  If the cell already contains a solid block
        /// nothing will happen. 
        /// </summary>
        /// <param name="cell">cell to build the block</param>
        /// <returns>true if block was just now built, otherwise false</returns>
        public bool BuildSolidBlock(Vector2 cell)
        {
            if (!IsCellEditable(cell)) return false;
            if (IsCellSolid(cell)) return false;
            
            SetCellv(cell, _defaultBlockID);
            return true;
        }

        /// <summary>
        /// Removes any solid blocks from this cell.  If there is no solid block, or the block is fixed (can't remove it)
        /// then nothing happens
        /// </summary>
        /// <param name="cell">cell to remove the block from</param>
        /// <returns>true if a block was just now removed from the cell, otherwise false</returns>
        public bool RemoveSolidBlock(Vector2 cell)
        {
            if (!IsCellEditable(cell)) return false;
            if (!IsCellSolid(cell)) return false;
            SetCellv(cell, -1);
            return true;
        }

        /// <summary>
        /// get whether this cell is locked or if it can be modified 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>true if this cell is allowed to be changed, otherwise false</returns>
        public bool IsCellEditable(Vector2 cell) => IsCellInsideBounds(cell);

        private bool IsCellInsideBounds(Vector2 cell)
        {
            
            if (cell.x <= _min.x)
            {
                Debug.Log($"Min x: c{cell.x} <= {_min.x}");
                return false;
            }
            
            if (cell.x >= _max.x)
            {
                Debug.Log($"Max x: c{cell.x} <= {_max.x}");
                return false;
            }

            if (cell.y <= _min.y)
            {
                Debug.Log($"Min y: c{cell.y} <= {_min.y}");
                return false;
            }

            if (cell.y >= _max.y)
            {
                Debug.Log($"Max y: c{cell.y} >= {_max.y}");
                return false;
            }
            
            return true;
        }


        public IEnumerable<Vector2> GetGasCellsInBlockCell(Vector2 blockCell)
        {
            var gasPerBlock = GasStuff.GasTilemap.steamTilesPerBlockTile;
            var gasStart = new Vector2(blockCell.x * gasPerBlock, blockCell.y * gasPerBlock);
            for (int i = 0; i < gasPerBlock; i++)
            {
                for (int j = 0; j < gasPerBlock; j++)
                {
                    yield return gasStart + new Vector2(i, j);
                }
            }
        }
    }
}
