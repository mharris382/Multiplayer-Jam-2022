using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot.Collections;
using JetBrains.Annotations;
//#define SORTED
//#define SHUFFLED
// #define NONE

public class GasController : Node
{
    [Export()]
    public int flowCapacity = 2;
    
    [Export()]
    NodePath gasTilemapPath = new NodePath();
    
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
        foreach (Vector2 cell in unvisited)
        {
            var gas = _gasTilemap.GetSteam(cell);

            StringBuilder sb2 = new StringBuilder();
            if (gas > 1)
            {
                var neighbors = new Godot.Collections.Array<Vector2>(_gasTilemap.GetLowerNeighbors(cell));
                neighbors.Shuffle();
                var cnt = 0;

                foreach (var neighbor in neighbors)
                {
                    if (!GasStuff.IsGasCellBlocked(neighbor))
                    {
                        cnt++;
                    }
                }

                if (cnt == 0)
                    continue;

                var amount = Mathf.Max(gas / cnt, 1);
                amount = Mathf.Min(amount, flowCapacity);
                
                
                #if true
                neighbors.ToList().Sort((t1, t2) =>
                {
                    var air1 = _gasTilemap.GetSteam(t1);
                    var air2 = _gasTilemap.GetSteam(t2);
                    if (air1 == air2)
                        return 0;
                    return air1 > air2 ? 1 : -1;
                });
                #elif true
                neighbors.Shuffle();
                #endif
                
                const int limit = 1;
                int ncnt = 0;
                foreach (var neighbor in neighbors)
                {
                    if (ncnt > limit)
                        break;
                    var neighborGas = _gasTilemap.GetSteam(neighbor);
                    var diffGas = gas - neighborGas;
                    if (!GasStuff.IsGasCellBlocked(neighbor) && neighborGas < gas)
                    {
                        _gasTilemap.TransferSteam(cell, neighbor, amount);
                        ncnt++;
                    }
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