using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Godot.Collections;
using JetBrains.Annotations;

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
        
        var gasTilemap = GetNodeOrNull<GasTilemap>(gasTilemapPath);
        var tileMap = GetNodeOrNull<TileMap>(gasTilemapPath);
        GD.Print(gasTilemap);
        GD.Print(tileMap);
        await GetTilemaps();
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
                sb2.Append("Found Neighbors: ");
                foreach (var neighbor in neighbors)
                {
                    if (!GasStuff.IsGasCellBlocked(neighbor))
                    {
                        sb2.Append(neighbor);
                        cnt++;
                    }
                }

                if (cnt == 0)
                {
                    continue;
                }

                sb2.AppendLine($"\t total neighbors = {cnt}");
                var amount = Mathf.Max(gas / cnt, 1);
                amount = Mathf.Min(amount, flowCapacity);
                neighbors.Shuffle();
                foreach (var neighbor in neighbors)
                {
                    var neighborGas = _gasTilemap.GetSteam(neighbor);
                    if (!GasStuff.IsGasCellBlocked(neighbor) && neighborGas < gas)
                    {
                        //Debug.Log("Trying to transfer");
                        _gasTilemap.TransferSteam(cell, neighbor, amount);
                    }
                    else
                    {
                        //Debug.Log("Blocked");
                    }
                }
            }

            //Debug.Log(sb2.ToString());
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


//timer callback, setup in scene
    [UsedImplicitly] public void _iterate_sources() => IterateSources();
}