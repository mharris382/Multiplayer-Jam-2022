using System;
using System.Collections.Generic;
using Game.blocks.gas;
using Game.core;
using Godot;
using JetBrains.Annotations;

namespace Game.Blocks.Gas.AirCurrents
{
    
    public class AirFlowTilemap : TileMap
    {
        
        public override void _Ready()
        {
            Flow.RunTests();
            base._Ready();
            
        }
        
    }
   
    internal static class Flow
    {
        const int DIRECTION_BIT_SHIFT = 1;
        public enum FlowStates
        {
            NONE = -1,
            OUT =1,
            IN = 0,
            OUT_R= OUT | (GridDirections.RIGHT << DIRECTION_BIT_SHIFT),
            OUT_U= OUT | (GridDirections.UP << DIRECTION_BIT_SHIFT),
            OUT_L= OUT | (GridDirections.LEFT << DIRECTION_BIT_SHIFT),
            OUT_D= OUT | (GridDirections.DOWN << DIRECTION_BIT_SHIFT),
            IN_R = IN | (GridDirections.RIGHT << DIRECTION_BIT_SHIFT),
            IN_U = IN | (GridDirections.UP << DIRECTION_BIT_SHIFT),
            IN_L = IN | (GridDirections.LEFT << DIRECTION_BIT_SHIFT),
            IN_D = IN | (GridDirections.DOWN << DIRECTION_BIT_SHIFT)
        }
         public static GridDirections GetDirection(this FlowStates states)
         {
             if (states == FlowStates.NONE) return GridDirections.NONE;
             int bit = (int)states;
             bit = bit >> DIRECTION_BIT_SHIFT;
             return (GridDirections)bit;
         }

         public static void RunTests()
         {
             Debug.Log("Running flow test");
             var arr = new (FlowStates @in, FlowStates @out, GridDirections dir)[]
             {
                (FlowStates.IN_D, FlowStates.OUT_D, GridDirections.DOWN),
                (FlowStates.IN_R, FlowStates.OUT_R, GridDirections.RIGHT),
                (FlowStates.IN_L, FlowStates.OUT_L, GridDirections.LEFT),
                (FlowStates.IN_U, FlowStates.OUT_U, GridDirections.UP)
             };
             foreach (var valueTuple in arr)
             {
                 if (!Debug.Assert(IsMatch(valueTuple.@in, valueTuple.@out, valueTuple.dir), "Right"))
                 {
                     Debug.Log("Failed test");
                 }
             }
             bool IsMatch(FlowStates @in, FlowStates @out, GridDirections dir)
             {
                 return @out.GetDirection() == dir &&
                        @out.GetDirection() == dir;
             }

            

             Debug.Assert(IsMatch(FlowStates.IN_R, FlowStates.OUT_R, GridDirections.RIGHT), "Right");
             
         }


         
         
    }


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