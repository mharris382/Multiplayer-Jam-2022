using System;
using System.Collections.Generic;
using Game.blocks.gas;
using Game.core;
using Godot;
using JetBrains.Annotations;

namespace Game.Blocks.Gas.AirCurrents
{
    
    public class AirFlowTileMap : TileMap
    {
        public override void _Ready()
        {
            Flow.RunTests();
            Flow.UpdateFlows(this, true);
            base._Ready();
        }


        static void RunTests()
        {
            
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


         
         
         public static void UpdateFlows(AirFlowTileMap flowTileMap, bool firstTime=false)
         {
             FlowStates[] flows = new[]
             {
                 FlowStates.OUT_R,
                 FlowStates.OUT_U,
                 FlowStates.OUT_L,
                 FlowStates.OUT_D,
                 FlowStates.IN_R,
                 FlowStates.IN_U,
                 FlowStates.IN_L,
                 FlowStates.IN_D
             };
             Dictionary<FlowStates, int> tileLookup = new Dictionary<FlowStates, int>();
             for (int i = 0; i < flows.Length; i++)
             {
                 tileLookup.Add(flows[i], i);
                 var tileName = flowTileMap.TileSet.TileGetName(i);
                 if(firstTime)
                    Debug.Log($"{flows[i]} matches with tile {tileName}");
             }

             FlowStates[] inStates = new FlowStates[]
             {
                 FlowStates.IN_R,
                 FlowStates.IN_U,
                 FlowStates.IN_L,
                 FlowStates.IN_D
             };
             
             Dictionary<Vector2, List<Vector2>> _validNeighbors = new Dictionary<Vector2, List<Vector2>>();
             foreach (var state in inStates)
             {
                 var inflowCell = flowTileMap.GetUsedCellsById(tileLookup[state]);
                 foreach (var vec in inflowCell)
                 {
                     var cell = (Vector2)vec;
                     _validNeighbors.Add(cell, new List<Vector2>());
                 }
             }

             foreach (var kvp in _validNeighbors)
             {
                 var cell = kvp.Key;
                 var actualNeighbors = cell.GetCellHandle().UnblockedNeighbors;
                 //GasStuff.GetPossibleBlockedDirections()
                 foreach (var neighbor in actualNeighbors)
                 {
                     kvp.Value.Add(neighbor.Position);
                 }
             }
         }  
    }
}