using System;
using System.Collections.Generic;
using Godot;

namespace Game.Blocks.Gas.AirCurrents
{
    public static class TilemapExtensions
    {
        private static Dictionary<int, int> _sizeCache = new Dictionary<int, int>(6);

        static TilemapExtensions()
        {
            _sizeCache.Add(128, 1);
            _sizeCache.Add(64, 2);
            _sizeCache.Add(32, 4);
            _sizeCache.Add(16, 8);
            _sizeCache.Add(8, 16);
            _sizeCache.Add(4, 32);
            _sizeCache.Add(2, 64);
            _sizeCache.Add(1, 128);
        }

        private static int GetNumberOfTilesPerBlockTile(this TileMap tileMap)
        {
            if ((int)tileMap.CellSize.x != (int)tileMap.CellSize.y)
                throw new InvalidTileMapSizeException(tileMap);
            if (_sizeCache.ContainsKey((int)tileMap.CellSize.x) == false) throw new InvalidTileMapSizeException(tileMap);
            return _sizeCache[(int)tileMap.CellSize.x];
        }

        private static int GetNumberOfGasTilesPerCell(this TileMap tileMap)
        {
            var gasPerBlock = GasStuff.GasTilemap.steamTilesPerBlockTile;
            var mapPerBlock = tileMap.GetNumberOfTilesPerBlockTile();
            if (gasPerBlock == mapPerBlock) return 1;
            if (mapPerBlock > gasPerBlock)
                throw new InvalidOperationException($"Why would you call this if gas is not the smallest resolution.\n map res = {mapPerBlock} \t gas res = {gasPerBlock} ");
            return gasPerBlock / mapPerBlock;
        }

        public static IEnumerable<Vector2> GetGasCellsOnCell(this TileMap map, Vector2 cell)
        {
            int gasPerCell = map.GetNumberOfGasTilesPerCell();
            if (gasPerCell == 1)
            {
                yield return cell;
            }
            else if(gasPerCell > 1)
            {
                var gasStart = new Vector2(cell.x * gasPerCell, cell.y * gasPerCell);
                for (int i = 0; i < gasPerCell; i++)
                {
                    for (int j = 0; j < gasPerCell; j++)
                    {
                        yield return gasStart + new Vector2(i, j);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
           
        }
        private class InvalidTileMapSizeException : Exception
        {
            public InvalidTileMapSizeException(TileMap tileMap) : base($"The tilemap {tileMap.Name} has non-standard cell size of {tileMap.CellSize}\n" +
                                                                       $"Valid Cell Sizes are: 128, 64, 32, 16, 8, 4, 2, 1"){}
        }
    }
}