using Godot;
using System;
using System.Collections.Generic;
using Game.core;

public class GasSourceTilemap : TileMap
{
    [Export()]
    public int rate = 6;
    
    // Called when the node enters the scene tree for the first time.
    private Dictionary<Vector2, ISteamSource> _sources = new Dictionary<Vector2, ISteamSource>();

    public override async void _Ready()
    {
        await GasStuff.WaitForAssignments();
        RegisterSources();
        Visible = false;
    }

    void RegisterSources()
    {
        foreach (var cell in GetUsedCells())
        {
            var v = (Vector2) cell;
            if (_sources.ContainsKey(v))
            {
                GasStuff.AddSource(_sources[v]);
            }
            else
            {
                var s = GetSource(v);
                _sources.Add(v, s);
                GasStuff.AddSource(s);
            }
        }
    }

    void UnregisterSources()
    {
        foreach (var kvp in _sources)
        {
            GasStuff.RemoveSource(kvp.Value);
        }
    }
    
    private GasSourceCell GetSource(Vector2 v)
    {
        return new GasSourceCell(this, v, rate);
    }
    
    private struct GasSourceCell : ISteamSource
    {
        public GasSourceCell(GasSourceTilemap tilemap, Vector2 cell, int output)
        {
            this.cell = cell;
            this.tilemap = tilemap;
        }

        private Vector2 cell;
        private readonly GasSourceTilemap tilemap;

        public int Output
        {
            get => tilemap.rate; 
            set { }
        }
        public bool Enabled
        {
            get => true;
            set { }
        }

        public Vector2 Position
        {
            get => cell;
            set { }
        }
        public void BroadcastSourceStateChanged()
        {
            Debug.Log("Still calling this?BroadcastSourceStateChanged");
        }

        public Vector2 GetWorldSpacePosition()
        {
            return tilemap.MapToWorld(cell);
        }
    }
}
