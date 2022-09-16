using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.blocks.gas;
using Game.core;
using Godot.Collections;
using JetBrains.Annotations;
using Array = Godot.Collections.Array;
using UnblockedNeighborsList = System.Collections.Generic.List<(Godot.Vector2 cell, int gasAmount)>;
using GasOutflowLookup = System.Collections.Generic.Dictionary<Godot.Vector2, int>;
using GasOutflowRecord = System.Collections.Generic.Dictionary<Godot.Vector2, System.Collections.Generic.Dictionary<Godot.Vector2, int>>;

//#define SORTED
//#define SHUFFLED
// #define NONE

public class GasController : Node
{
    [Export()]
    public int flowCapacity = 2;
    
    [Export()]
    NodePath gasTilemapPath = new NodePath();

    [Export()]
    public bool freezeSimulation = false;

    [Export()]
    public bool skipOnInequalNeighbors = false;
    
    
    private GasTilemap _gasTilemap;
    private SolidBlockTilemap _blockTilemap;
    
    private bool _valid = false;
    private bool _refreshAirspaces = false;

    /// <summary>
    /// resolves all dependencies prior to running the cellular automata algorithm
    /// </summary>
    public override async void _Ready()
    {
        await GetTilemaps();
        GasStuff.ReBuildGraphs();
        Debug.Log($"Found {GasStuff.Graphs.Count} connected air spaces");
        
    }

    /// <summary>
    /// waits for a gas tilemap to be assigned to GasStuff
    /// <see cref="GasStuff"/>
    /// <seealso cref="SolidBlockTilemap"/>
    /// <seealso cref="GasTilemap"/>
    /// </summary>
    private async Task GetTilemaps()
    {
        _valid = false;
        Debug.Log("Getting Tilemaps");
        await GasStuff.WaitForAssignments();
        Debug.Log("Finished finding Tilemaps");
        _blockTilemap = GasStuff.BlockTilemap;
        _gasTilemap = GasStuff.GasTilemap;
        if (!Debug.AssertNotNull(_blockTilemap)) return;
        if (!Debug.AssertNotNull(_gasTilemap)) return;
        _valid = true;
        Debug.Log("Successfully found Tilemaps");
    }

    /// <summary>
    /// cellular automata algorithm
    /// </summary>
    private void IterateSources()
    {
        if (!_valid)
        {
            Debug.Log("Invalid Gas");
            return;
        }
        
        AddGas();
        RemoveGasFromSinks();
        if(!freezeSimulation)
            DiffuseGas();
        GasStuff.GasIteration++;
    }

    private void RemoveGasFromSinks()
    {
        foreach (var sink in GasStuff.GetSinks())
        {
            var amount = _gasTilemap.RemoveSteam(sink.Item1, sink.Item2);
            if (amount > 0)
            {
                Debug.Log($"Removed {amount} from {sink.Item1}");
            }
        }
    }

    private void DiffuseGas()
    {
        var unvisited = new List<Vector2>();
        var lastStateLookup = new Array<Vector2>[16];
        // var sb = new StringBuilder();
        for (int i = 0; i < 16; i++)
        {
            lastStateLookup[i] = _gasTilemap.GetUsedCellsById(i);
            unvisited.AddRange(lastStateLookup[i]);
            // sb.AppendLine($"Found {lastStateLookup[i].Count} gas tiles with pressure = {i}");
        }

        DiffuseGas(unvisited);
    }
    
    private void DiffuseGas(List<Vector2> unvisited)
    {
        GasOutflowRecord outflows = new GasOutflowRecord(); //record of gas outflow from each cell
        UnblockedNeighborsList unblockedNeighbors; //variable to store list of unblocked neighbor cells
        
        foreach (Vector2 cell in unvisited)
        {
            const int flowLimit = 5;
            
            //query for gas amount of current cell
            var gasAmount = _gasTilemap.GetSteam(cell);
            
            //keep track of amount of gas which has moved this turn
            int curOutflow = 0;
            
            
            //TODO: this dictionary could very easily be replaced with using a direction to index mapping 
            //create outflow dictionary lookup 
            outflows.Add(cell, new System.Collections.Generic.Dictionary<Vector2, int>());

            //get a list of unblocked neighbors, if none are found continue
            if (!TryGetValidNeighbors(cell, out var neighbors)) continue;
            
            //if has any empty neighbors transfer gas to one of the empty cells (doesn't matter which one)
            CheckForEmptyNeighbors(cell, neighbors);
            gasAmount = _gasTilemap.GetSteam(cell);
      
            
            //only continue if has 1 or more gas
            // if (CheckForInequalNeighbors(cell, neighbors))
            // {
            //     
            // }
            
            if (gasAmount <= 1) continue;

            
            
            
            
            foreach (var neighbor in neighbors)
            {
                gasAmount = _gasTilemap.GetSteam(cell);
                if (curOutflow >= flowLimit)
                    break;
                
                var gasDiff = gasAmount - neighbor.gasAmount;
                if (gasDiff > 1)
                {
                    
                    var transferAmount = gasDiff > 2 ? Mathf.CeilToInt(gasDiff / 2.0f) : 1;
                    TransferAmountAndRecordOutflow(cell,neighbor.cell, transferAmount);
                    
                    
                    if(HasOutflow(cell, neighbor.cell))
                        curOutflow += outflows[cell][neighbor.cell];
                }
            }
        }
        
        bool TryGetValidNeighbors(Vector2 cell, out UnblockedNeighborsList neighbors)
        {
            neighbors = GasStuff.GetUnblockedNeighbors(cell).ToList();
            var cnt = neighbors.Count;
            if (cnt == 0)
                return false;
            neighbors.Sort((t1, t2) =>
            {
                var air1 = _gasTilemap.GetSteam(t1.cell);
                var air2 = _gasTilemap.GetSteam(t2.cell);
                if (air1 == air2) return 0;
                return air1 > air2 ? 1 : -1;
            });
            return true;
        }



        bool HasOutflow(Vector2 from, Vector2 to)
        {
            if (outflows.ContainsKey(from) == false)
                return false;
            if (outflows[from].ContainsKey(to) == false)
                return false;
            return true;
        }

       
        void TransferAmountAndRecordOutflow(Vector2 from, Vector2 to, int amount)
        {
            if (outflows[from].ContainsKey(to))
                return;
            
            // CellHandle.TransferGas(from, to, amount);
            if (_gasTilemap.TransferSteam(from, to, ref amount)) 
                outflows[from].Add(to, amount);
        }


        void CheckForEmptyNeighbors(Vector2 cell, UnblockedNeighborsList valueTuples)
        {
            int emptyCount = GasSim.GetEmptyNeighbors(valueTuples, out var emptyNeighbors);
            if (emptyCount > 0)
            {
                if (rng.RandiRange(1, 5) <= 2)
                {
                    TransferAmountAndRecordOutflow(cell, emptyNeighbors[rng.RandiRange(0, emptyCount - 1)], 1);
                }
                else
                {
                    TransferAmountAndRecordOutflow(cell, emptyNeighbors[0], 1);
                }
            }
        }

        bool CheckForInequalNeighbors(Vector2 cell, UnblockedNeighborsList valueTuples)
        {
            var cellHandle = cell.GetCellHandle();
            var average = (int)valueTuples.Average(t => t.gasAmount);
            bool found = false;
            foreach (var valueTuple in valueTuples.Where(t => t.gasAmount < cellHandle.GasAmount))
            {
                if (valueTuple.gasAmount < average )
                {
                    TransferAmountAndRecordOutflow(cell, valueTuple.cell, 1);
                    found = true;
                }
            }
            // if (cellHandle.GasAmount != average)
            // {
            //     average = (average + cellHandle.GasAmount) / 2;
            // }
            // else
            // {
            //     
            // }
            
            return found;
        }

    }
    RandomNumberGenerator  rng = new Godot.RandomNumberGenerator();
    private void AddGas()
    {
        foreach (var gasToAdd in GasStuff.GetGasFromSourcesToAddToSystem())
        {
            if (_gasTilemap.ModifySteam(gasToAdd.Item1, gasToAdd.Item2, out var added))
            {
                //Debug.Log($"ADDED GAS TO SIM: {added}");
                // Logger.Log2($"Added {added}");
            }
        }
    }



    public void _on_Clear_Button_pressed()
    {
        _gasTilemap.Clear();
    }
//timer callback, setup in scene
    [UsedImplicitly] public void _iterate_sources() => IterateSources();
}