using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class DiffusionGraph : Node
{

    const string MODIFY_STEAM ="modify_steam";
    const string GET_STEAM ="get_steam";
    const string SET_STEAM = "set_steam";


    
    TileMap _steamTilemap;
    TileMap _blockTilemap;

    List<Tree> _trees = new List<Tree>();
    
    HashSet<Vector2> _visited_nodes = new HashSet<Vector2>();
    Stack<GasCell> _unvisited_nodes = new Stack<GasCell>();

    SteamTileMapWrapper _steamTileMapWrapper;

    Dictionary<Vector2, GasCell> gasCells = new Dictionary<Vector2, GasCell>();
    

    Vector2[] Directions4 = new Vector2[]{
        Vector2.Down,
        Vector2.Up,
        Vector2.Left,
        Vector2.Right
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _steamTilemap = GetNode<TileMap>(new NodePath("../Steam TileMap"));
        _blockTilemap = GetNode<TileMap>(new NodePath("../Block TileMap"));
        var timer = GetNode<Timer>(new NodePath("../IterationTimer"));
        timer.Connect("timeout", this, "_Iterate");
    }


    public void _Iterate(){

        _visited_nodes.Clear();
        _unvisited_nodes.Clear();
        _trees.Clear();

        for(int i = 16; i > 0; i--){
            var arr = _steamTilemap.GetUsedCellsById(i-1);
            if (arr != null && arr.Count > 0)
            {
                foreach(var a in arr)
                {
                    var gasCell = new GasCell((Vector2)a, GetSteam((Vector2)a));
                   _unvisited_nodes.Push(gasCell);
                   if (!gasCells.ContainsKey(gasCell.location))
                        gasCells.Add(gasCell.location, gasCell);
                }
            }
        }

        while(_unvisited_nodes.Count > 0)
        {
            var next = _unvisited_nodes.Pop();
            if(!_visited_nodes.Contains(next.location))
            {
                Tree currentTree = new Tree();
                Visit(currentTree, next.location);
                _trees.Add(currentTree);
            }
        }
        GD.Print($"Found {_trees.Count} trees");
        for(int i = 0; i < _trees.Count; i++)
        {
            var tree = _trees[i];
            GD.Print($"Tree{i} has \na total area = {tree.total_space}\ntotal gas = {tree.total_gas}");
        }
        
    }

    private void Visit(Tree currentTree, Vector2 cell)
    {
        if(_visited_nodes.Contains(cell))
            return;
        
        _visited_nodes.Add(cell);
        currentTree.total_space++;
        currentTree.total_gas+= GetSteam(cell);
        currentTree.cells.Add(cell);

        var neighbors = from dir in Directions4 
                where !IsCellBlocked(cell+dir)&&!_visited_nodes.Contains(cell+dir) 
                select dir + cell;
        
        if(neighbors == null)
            return;
        
        foreach (var neighbor in neighbors)
        {
            Visit(currentTree, neighbor);
        }
    }

    private void Visit(Tree current_tree, GasCell cell)
    {

    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    struct GasCell
    {
        public Vector2 location;
        public int gas;
        public GasCell(Vector2 location, int gas)
        {
            this.gas = gas;
            this.location = location;
        }
    }
    class Tree
    {
        public int total_space;
        public int total_gas;
        public List<Vector2> cells = new List<Vector2>();
    }


    public bool IsCellBlocked(Vector2 position) => _blockTilemap.GetCellv(position)!= -1;

    public int ModifySteam(Vector2 position, int amount) => (int)(_steamTilemap.Call(MODIFY_STEAM, position.x, position.y, amount));
    public int GetSteam(Vector2 position) => (int)(_steamTilemap.Call(GET_STEAM, position.x, position.y));
    public void SetSteam(Vector2 position, int value) => _steamTilemap.Call(SET_STEAM, position.x, position.y, value);
    class SteamTileMapWrapper
    {
        
    }
}
