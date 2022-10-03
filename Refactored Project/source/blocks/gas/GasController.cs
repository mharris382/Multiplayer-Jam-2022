using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Blocks.Fluid;
using Game.blocks.gas;
using Game.Blocks.Gas;
using Game.Blocks.Gas.Tilemaps;
using Game.core;
using Godot.Collections;
using JetBrains.Annotations;
using Array = Godot.Collections.Array;
using Debug = Game.core.Debug;
using UnblockedNeighborsList = System.Collections.Generic.List<(Godot.Vector2 cell, int gasAmount)>;
using GasOutflowLookup = System.Collections.Generic.Dictionary<Godot.Vector2, int>;
using GasOutflowRecord =
    System.Collections.Generic.Dictionary<Godot.Vector2, System.Collections.Generic.Dictionary<Godot.Vector2, int>>;

//#define SORTED
//#define SHUFFLED
// #define NONE

public class GasController : Node
{
    public bool autoStart = true;
    [Export()] public int flowCapacity = 2;

    [Export()] public bool runFluidSimulation;
    [Export()] NodePath gasTilemapPath = new NodePath();
    [Export()] private NodePath sourceTilemapPath = new NodePath();
    [Export()] private NodePath sinkTilemapPath = new NodePath();
    [Export()] public bool skipOnUnequalNeighbors = false;


    private GasTilemap _gasTilemap;
    private SolidBlockTilemap _blockTilemap;

    private readonly RandomNumberGenerator _rng = new Godot.RandomNumberGenerator();
    private bool _valid = false;
    private Task _currentTask;

    private Stopwatch _simulationWatch = new Stopwatch();
    private FluidSimulation _fluidSimulation;

    /// <summary>
    /// resolves all dependencies prior to running the cellular automata algorithm
    /// </summary>
    public override async void _Ready()
    {
        await GetTilemaps();
        if (!Debug.AssertNotNull(_blockTilemap)) return;
        if (!Debug.AssertNotNull(_gasTilemap)) return;
        var sinkTilemap = GetNode<SinkTileMap>(sinkTilemapPath);
        var sourceTilemap = GetNode<GasSourceTilemap>(sourceTilemapPath);
        Debug.AssertNotNull(sinkTilemap);
        Debug.AssertNotNull(sourceTilemap);
        var builder = new FluidSimulationBuilder(_gasTilemap, _blockTilemap,
            sourceTilemap: sourceTilemap,
            sinkTileMap: sinkTilemap);
        _fluidSimulation = builder.BuildSimulation();
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

    private int cnt = 0;
    private void UpdateSimulation()
    {
        if (runFluidSimulation)
        {
            IterateFluidSim();
        }
        else
        {
            
            _simulationWatch.Restart();
            IterateGasSim();
            _simulationWatch.Stop();
            if (GasStuff.GasTilemap == null) return;
            int cellCount = GasStuff.GasTilemap.GetUsedCells().Count;
            float rate = _simulationWatch.ElapsedMilliseconds/ (float)cellCount;
            
            Debug.Log($"ITERATION: \t # of CELLS: {cellCount} \t TIME: {_simulationWatch.ElapsedMilliseconds} MS\t {rate:F4} ms per cell");
        }
    }

    private void IterateFluidSim()
    {
        if (_fluidSimulation == null)
        {
            if (_gasTilemap == null || _blockTilemap == null)
            {
                return;
            }
            var sinkTilemap = GetNode<SinkTileMap>(sinkTilemapPath);
            var sourceTilemap = GetNode<GasSourceTilemap>(sourceTilemapPath);
            Debug.AssertNotNull(sinkTilemap);
            Debug.AssertNotNull(sourceTilemap);
            var builder = new FluidSimulationBuilder(_gasTilemap, _blockTilemap,
                sourceTilemap: sourceTilemap,
                sinkTileMap: sinkTilemap);
            _fluidSimulation = builder.BuildSimulation();
        }
        else
        {
            _fluidSimulation.UpdateSimulation();
        }
    }

    /// <summary>
    /// cellular automata algorithm
    /// </summary>
    private void IterateGasSim()
    {
        if (!_valid)
        {
            Debug.Log("Invalid Gas");
            return;
        }

        var gasIOs = GasStuff.GetSinks().Select(t => (t.Item1, -t.Item2)).Concat(GasStuff.GetSources());
        var graph = new Graph<Vector2>(GraphType.UNDIRECTED_UNWEIGHTED);
        IEnumerable<(Vector2, int)> gasCells = GasStuff.GetAllGasCells();
        AddGas();
        RemoveGasFromSinks();
        DiffuseGas();
        PullTowardsSinks();
        GasStuff.GasIteration++;
    }

    private void RemoveGasFromSinks()
    {
        foreach (var sink in GasStuff.GetSinks())
        {
            var amount = _gasTilemap.RemoveSteam(sink.Item1, sink.Item2);
            if (amount > 0)
            {
                //Debug.Log($"Removed {amount} from {sink.Item1}");
            }
        }
    }


    private void PullTowardsSinks()
    {
        foreach (var pullingCell in GasStuff.GetPullingCells())
        {
            var neighbors = pullingCell.cell.GetCellHandle().UnblockedNeighbors.Where(t => t.GasAmount > 0).ToList();
            foreach (var neighbor in neighbors)
            {
                if (neighbor.GasAmount <= 1)
                {
                    GasStuff.GasTilemap.MoveSteam(pullingCell.cell, neighbor.Position, 1);
                }
                else
                {
                    var amt = neighbor.GasAmount / 2;
                    GasStuff.GasTilemap.MoveSteam(pullingCell.cell, neighbor.Position, amt);
                }
            }
        }
    }


    /// <summary>
    /// selects the order to traverse the graph <seealso cref="DiffuseGas()"/>
    /// </summary>
    private void DiffuseGas()
    {
        var unvisited = new List<Vector2>();
        var lastStateLookup = new Array<Vector2>[16];
        // var sb = new StringBuilder();
        for (int i = 0; i < 16; i++)
        {
            lastStateLookup[i] = _gasTilemap.GetUsedCellsById(i);
            unvisited.AddRange(lastStateLookup[i]);
        }

        GasOutflowRecord outflows = new GasOutflowRecord();
        UnblockedNeighborsList unblockedNeighbors;

        int curOutflow = 0;
        foreach (Vector2 cell in unvisited)
        {
            const int flowLimit = 5;

            outflows.Add(cell, new System.Collections.Generic.Dictionary<Vector2, int>());


            UnblockedNeighborsList neighbors;
            if (!TryGetValidNeighbors(cell, out neighbors)) continue;

            if (CheckForEmptyNeighbors(cell, neighbors))
            {
            }
            else if (CheckForLowerNeighbors(cell, neighbors))
            {
            }
            else
            {
                curOutflow = FlowToNeighbors(cell, neighbors, flowLimit);
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
            {
                var remCap = flowCapacity - outflows[from][to];
                if (remCap > 0)
                {
                    amount = Mathf.Min(remCap, amount);
                    var curFlow = outflows[from][to];
                    curFlow += _gasTilemap.MoveSteam(from, to, amount);
                    outflows[from][to] = curFlow;
                }
            }
            else
            {
                outflows[from].Add(to, _gasTilemap.MoveSteam(from, to, amount));
            }
        }

        bool CheckForEmptyNeighbors(Vector2 cell, UnblockedNeighborsList valueTuples)
        {
            int emptyCount = GasSim.GetEmptyNeighbors(valueTuples, out var emptyNeighbors);
            if (emptyCount > 0)
            {
                if (_rng.RandiRange(1, 5) <= 2)
                {
                    TransferAmountAndRecordOutflow(cell, emptyNeighbors[_rng.RandiRange(0, emptyCount - 1)], 1);
                }
                else
                {
                    TransferAmountAndRecordOutflow(cell, emptyNeighbors[0], 1);
                }

                return true;
            }

            return false;
        }

        bool CheckForLowerNeighbors(Vector2 cell, UnblockedNeighborsList valueTuples)
        {
            int lowerCount = GasSim.GetLowerNeighbors(cell.GetCellHandle().GasAmount, valueTuples,
                out Array<Vector2> lowerNeighbors);
            if (lowerCount > 0)
            {
                if (_rng.RandiRange(1, 5) <= 2)
                {
                    TransferAmountAndRecordOutflow(cell, lowerNeighbors[_rng.RandiRange(0, lowerCount - 1)], 1);
                }
                else
                {
                    TransferAmountAndRecordOutflow(cell, lowerNeighbors[0], 1);
                }

                return true;
            }

            return false;
        }

        int FlowToNeighbors(Vector2 cell, UnblockedNeighborsList neighbors, int flowLimit)
        {
            var gasAmount = _gasTilemap.GetSteam(cell);
            if (gasAmount <= 1) return curOutflow;
            foreach (var neighbor in neighbors)
            {
                gasAmount = _gasTilemap.GetSteam(cell);
                if (curOutflow >= flowLimit)
                    break;

                var gasDiff = gasAmount - neighbor.gasAmount;
                if (gasDiff > 1)
                {
                    var transferAmount = gasDiff > 2 ? Mathf.CeilToInt(gasDiff / 2.0f) : 1;
                    var toCell = neighbor.cell;
                    var fromCell = cell;
                    var direction = toCell - fromCell;
                    //GasSim.TransferGas(fromCell, toCell, transferAmount);
                    TransferAmountAndRecordOutflow(cell, neighbor.cell, transferAmount);


                    if (HasOutflow(cell, neighbor.cell))
                        curOutflow += outflows[cell][neighbor.cell];
                }
            }

            return curOutflow;
        }
    }

    private void AddGas()
    {
        foreach (var gasToAdd in GasStuff.GetGasFromSourcesToAddToSystem())
        {
            if (_gasTilemap.AddGasFromSource(gasToAdd.Item1, gasToAdd.Item2) > 0)
            {
            }
        }
    }


    public void _on_Clear_Button_pressed()
    {
        _gasTilemap.Clear();
    }

//timer callback, setup in scene
    [UsedImplicitly]
    public void _iterate_sources()
    {
        UpdateSimulation();
        // if (_currentTask != null)
        // {
        //     if (!_currentTask.IsCompleted)
        //     {
        //        // Debug.Log("Awaiting iteration!");
        //         await _currentTask;
        //     }
        // }
        // _currentTask = Task.Run(IterateSources);
    }

    #region [Playing with code]

#if PLAY_WITH_CODE
        private PriorityQueue<Vector2> priorityQueue;
    private Graph<Vector2> airspaceGraph;
    private Graph<Vector2> airDensityGraph;
    
    private HashSet<Vector2>[] gasCells;
    private System.Collections.Generic.Dictionary<Vector2, int> dict;
    private void GasTest()
    {
        const int CAP = 1000000;
        
        priorityQueue = new PriorityQueue<Vector2>(CAP);
        
        airspaceGraph = new Graph<Vector2>(GraphType.UNDIRECTED_UNWEIGHTED);
        
        gasCells = new HashSet<Vector2>[16];
        dict = new System.Collections.Generic.Dictionary<Vector2, int>();
        HashSet<Vector2> usedCells = new HashSet<Vector2>();
        foreach (var cell in GasStuff.GasTilemap.GetUsedCells())
        {
            usedCells.Add(cell);
        }
        //empty unblocked cells that have one or more neighbor that is a non-empty gas cell
        HashSet<Vector2>[] airspaceEdgeCells = new HashSet<Vector2>[16];
        for (int i = 0; i < 16; i++)
        {

            gasCells[i] = new HashSet<Vector2>();
            var airspaceEdges = new HashSet<Vector2>();
            airspaceEdgeCells[i] = airspaceEdges;
           
            
            foreach (var cell in  GasStuff.GasTilemap.GetUsedCellsById(i))
            {
                bool IsNeighborUnblockedAndEmpty(Vector2 offset)
                {
                    var neighbor = cell + offset;
                    if (GasStuff.IsGasCellBlocked(neighbor) || usedCells.Contains(neighbor))
                    {
                        return false;
                    }
                    return true;
                }

                void CheckEdge(Vector2 offset)
                {
                    
                   bool result = !airspaceEdges.Contains(cell + Vector2.Right) &&
                        IsNeighborUnblockedAndEmpty(Vector2.Right);
                   if (result) airspaceEdges.Add(offset + cell);
                }
               
                CheckEdge(Vector2.Right);
                CheckEdge(Vector2.Left);
                CheckEdge(Vector2.Up);
                CheckEdge(Vector2.Down);
                
                gasCells[i].Add(cell);
                dict.Add(cell, i+1);
                priorityQueue.Enqueue(cell, i+1);
            }
            
            foreach (var cell in gasCells[i])
            {
                priorityQueue.Enqueue(cell, i);
            }
        }
        
        
        airDensityGraph = new Graph<Vector2>(GraphType.DIRECTED_WEIGHTED);
    }
    
    


    struct CellInfo
    {
        private Vector2 position;
        private Vector2 Up => position + Vector2.Up;
        private Vector2 Down => position + Vector2.Down;
        private Vector2 Right => position + Vector2.Right;
        private Vector2 Left => position + Vector2.Left;
        
        private Neighbors _neighbors;
        
        
        public CellInfo(Vector2 position)
        {
            _neighbors = 0;
            this.position = position;
            _neighbors += (GasStuff.IsGasCellBlocked(Right) ? (int)Neighbors.HAS_RIGHT : 0) +
                          (GasStuff.IsGasCellBlocked(Up) ? (int)Neighbors.HAS_UP : 0) +
                          (GasStuff.IsGasCellBlocked(Down) ? (int)Neighbors.HAS_DOWN : 0) +
                          (GasStuff.IsGasCellBlocked(Left) ? (int)Neighbors.HAS_LEFT : 0);
            int neighborEncoding = (int)_neighbors;
            
            int GetNeighborCount()
            {
           // int r = ((int)_neighbors & (int)Neighbors.HAS_RIGHT) >> RIGHT_BITSHIFT
           // int l = ((int)_neighbors & (int)Neighbors.HAS_LEFT) >> LEFT_BITSHIFT
           // int u = ((int)_neighbors & (int)Neighbors.HAS_UP) >> UP_BITSHIFT
           // int d = ((int)_neighbors & (int)Neighbors.HAS_DOWN) >> DOWN_BITSHIFT
           return
               (neighborEncoding & (int)Neighbors.HAS_RIGHT) >> RIGHT_BITSHIFT +
               (neighborEncoding & (int)Neighbors.HAS_LEFT) >> LEFT_BITSHIFT +
               (neighborEncoding & (int)Neighbors.HAS_UP) >> UP_BITSHIFT +
               (neighborEncoding & (int)Neighbors.HAS_DOWN) >> DOWN_BITSHIFT;
        }
        }


        

        const int LEFT_BITSHIFT = 1;
        const int RIGHT_BITSHIFT = 2;
        const int UP_BITSHIFT = 3;
        const int DOWN_BITSHIFT = 4;
        enum Neighbors
        {
            HAS_LEFT = 1 << LEFT_BITSHIFT,
            HAS_RIGHT = 1 << RIGHT_BITSHIFT,
            HAS_UP = 1 << UP_BITSHIFT,
            HAS_DOWN = 1 << DOWN_BITSHIFT
        }
    }
#endif

    #endregion
}