using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Blocks.Fluid;
using Game.Blocks.Gas;
using Game.core;

public class GasSourceTilemap : TileMap, FluidIO.ISources
{
    [Export()]
    public int rate = 6;
    
    // Called when the node enters the scene tree for the first time.
    private Dictionary<Vector2, ISteamSource> _sources = new Dictionary<Vector2, ISteamSource>();

    public void SetConverter(IGasToBlockConverter converter)
    {
        this.converter = converter;
    }
    private bool ready = false;
    private List<CellSource> _fixedSources = new List<CellSource>();
    private IGasToBlockConverter converter;

    public override async void _Ready()
    {
        await Task.Run(() =>
        {
            foreach (var cell in GetUsedCells())
            {
                var v = (Vector2)cell;
                _fixedSources.Add(new CellSource(v, (byte)(GetCellv(v) + 1)));
            }
            Debug.Log($"Gas Sources Done! Count = {_fixedSources.Count}\n Sum = {_fixedSources.Sum(t => t.Rate)}");
        });
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
            this.size = Vector2.One;
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

        public Vector2 size { get; }

        public void BroadcastSourceStateChanged()
        {
            Debug.Log("Still calling this?BroadcastSourceStateChanged");
        }

        public Vector2 GetWorldSpacePosition()
        {
            return tilemap.MapToWorld(cell);
        }
    }


    public IEnumerable<CellSource> GetActiveSources(Predicate<Vector2Int> condition)
    {
        
        var res = (from c in _fixedSources
            where condition(c.Position)
            select c);
        foreach (var cellSource in res)
        {
            yield return cellSource;
        }
        
    }
}
