using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot.Collections;
using JetBrains.Annotations;
using Dict = System.Collections.Generic.Dictionary<Godot.Vector2, System.Collections.Generic.Dictionary<Godot.Vector2, int>>;

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
    
    
    
    private GasTilemap _gasTilemap;
    private SolidBlockTilemap _blockTilemap;
    
    private bool _valid = false;


    /// <summary>
    /// resolves all dependencies prior to running the cellular automata algorithm
    /// </summary>
    public override async void _Ready()
    {
        // var gasTilemap = GetNodeOrNull<GasTilemap>(gasTilemapPath);
        // var tileMap = GetNodeOrNull<TileMap>(gasTilemapPath);
        // GD.Print(gasTilemap);
        // GD.Print(tileMap);
        await GetTilemaps();
        var graphs = GasStuff.BuildGraphs();
        Debug.Log($"Found {graphs.Count} connected air spaces");
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
        if(!freezeSimulation)
            DiffuseGas();
        return;
    }

    private void DiffuseGas()
    {
        var unvisited = new List<Vector2>();
        var lastStateLookup = new Array<Vector2>[16];
        var sb = new StringBuilder();
        for (int i = 0; i < 16; i++)
        {
            lastStateLookup[i] = _gasTilemap.GetUsedCellsById(i);
            unvisited.AddRange(lastStateLookup[i]);
            sb.AppendLine($"Found {lastStateLookup[i].Count} gas tiles with pressure = {i}");
        }

        DiffuseGas(unvisited);
    }
    
    private void DiffuseGas(List<Vector2> unvisited)
    {
        bool TryGetValidNeighbors(Vector2 cell, out List<(Vector2 cell, int gasAmount)> neighbors)
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


        Dict outflows = new Dict(); 

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
            if (_gasTilemap.TransferSteam(from, to, ref amount)) 
                outflows[from].Add(to, amount);
        }
        
        foreach (Vector2 cell in unvisited)
        {
            outflows.Add(cell, new System.Collections.Generic.Dictionary<Vector2, int>());
            
            var gasAmount = _gasTilemap.GetSteam(cell);
            if (gasAmount <= 1) continue;
            
            if (!TryGetValidNeighbors(cell, out var neighbors)) continue;

            const int flowLimit = 5;
            int curOutflow = 0;

            foreach (var neighbor in neighbors)
            {
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
    }

    private void AddGas()
    {
        foreach (var gasToAdd in GasStuff.GetGasFromSourcesToAddToSystem())
        {
            if (_gasTilemap.ModifySteam(gasToAdd.Item1, gasToAdd.Item2, out var added))
            {
                //Debug.Log($"ADDED GAS TO SIM: {added}");
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