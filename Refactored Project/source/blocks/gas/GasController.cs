using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Godot.Collections;

public class GasController : Node
{
    [Export()]
    public NodePath pathToGasTilemap = "./Gas TileMap";
    [Export()]
    public NodePath pathToBlockTilemap = "./Block TileMap";

    private GasTilemap _gasTilemap;
    private bool valid = false;
    
    
    
    public override async void _Ready()
    {
        await WaitForGasTilemapAssignment();
        valid = !Debug.AssertNotNull(_gasTilemap);
    }

    private async Task WaitForGasTilemapAssignment()
    {
        await Task.Run(() =>
        {
            int cnt = 0;
            while (GasStuff.ActiveGasTilemap == null || cnt < 100)
            {
                cnt++;
                Task.Delay(100).Wait();
            }
            _gasTilemap = GasStuff.ActiveGasTilemap;
        });
    } 
    
    public void IterateSources()
    {
        if (!valid)
        {
            Debug.Log("Invalid Gas");
            return;
        }

        var unvisited = new List<Vector2>();
        var lastStateLookup = new Array<Vector2>[16];
        var sb = new StringBuilder();
        for (int i = 0; i < 16; i++)
        {
            lastStateLookup[i] = _gasTilemap.GetUsedCellsById(i);
            unvisited.AddRange(lastStateLookup[i]);
            sb.AppendLine($"Found {lastStateLookup[i].Count} gas tiles with pressure = {i}");
        }
        Debug.Log(sb.ToString());
        
    }


    public void _iterate_sources() => IterateSources();
}