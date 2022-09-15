
using System;
using System.Collections.Generic;
using System.Linq;
using Game.core;
using Godot;

public static partial class GasStuff
{
    static List<AirspaceGraph> graphs = new List<AirspaceGraph>();

    static Dictionary<Vector2, int> trees = new Dictionary<Vector2, int>();

    public static List<AirspaceGraph> Graphs => graphs;


    public static bool HasAirspaceBeenFound(Vector2 position)
    {
        return trees.ContainsKey(position);
    }

    public static int FindAirspaceID(Vector2 position)
    {
        if (IsGasCellBlocked(position))
            return -1;

        if (!HasAirspaceBeenFound(position)) //this is the first time this position has been searched
        {
            Debug.Log($"has not searched {position}, so need to rebuild graphs");
            throw new NotImplementedException();
        }

        return trees[position];
    }

    public static AirspaceGraph GetAirspace(Vector2 position)
    {
        if (IsGasCellBlocked(position)) return null;
        return graphs[FindAirspaceID(position)];
    }

    public static AirspaceGraph GetAirspace(int id) => graphs[id];


    public const int BLOCKED_ID = -1;


    public static int RebuildFromCells(params Vector2[] sourceCells)
    {
        int spacesFound = 0;
        ForgetAirSpaces(sourceCells);
        foreach (var sourceCell in sourceCells)
        {
            if (IsGasCellBlocked(sourceCell)) continue;
            if (HasAirspaceBeenFound(sourceCell)) continue;
            spacesFound++;
            AddAirspaceGraph(sourceCell);
        }

        return spacesFound;
    }

    public static void ForgetAirspace(Vector2 position)
    {
        if (HasAirspaceBeenFound(position) == false) return;
        GetAirspace(position).Dispose();
    }

    public static void ForgetAllAirSpaces()
    {
        var arr = graphs.ToArray();
        foreach (var graph in arr)
            graph.Dispose();
    }

    public static void ForgetAirSpaces(params Vector2[] positions)
    {
        foreach (var pos in positions)
            ForgetAirspace(pos);
    }

    public static int ReBuildGraphs()
    {
        ForgetAllAirSpaces();
        var sourceCells = Sources.Select(t => GasTilemap.WorldToMap(t.Position));
        RebuildFromCells(GasTilemap.GetUsedCells().Union(sourceCells).Where(t => !trees.ContainsKey(t)).ToArray());
        return graphs.Count;
    }

    static void AddAirspaceGraph(Vector2 source)
   {
       if (HasAirspaceBeenFound(source)) return;
        if (Debug.Assert(trees.ContainsKey(source), $"Error trying to build tree on cell {source} twice!"))
            return;
            
        if (Debug.Assert(!IsGasCellBlocked(source), $"Error {source} cell is blocked!"))
            //TODO: need to remove any air in this cell.
            return;
            
        int id = graphs.Count;
        graphs.Add(new AirspaceGraph(id, source, GasTilemap.GetSteam(source)));
        DFS(graphs[id], source);    
        Debug.Log(graphs[id].ToString());
    }
    
   static void DFS(AirspaceGraph graph, Vector2 source)
    {
        if (trees.ContainsKey(source)) //if already traversed then do nothing
            return;
            
        bool isBlocked = IsGasCellBlocked(source);
        if (!isBlocked)
        {
            //mark cell as searched
            trees.Add(source, graph.graphID);
                
            //update airspace cell from tilemap
            graph.SetAirspaceCell(source,  GasTilemap.GetSteam(source));
                
            //iterate through unblocked? neighbors
            foreach (var neighbor in GetNeighbors(source))
            {
                DFS(graph, neighbor);
            }
        }
        else
        {
            //mark cell as searched
            trees.Add(source, BLOCKED_ID);
        }
           
    }
    //--------------------------------------------------------------------------

    public class AirspaceGraph : IDisposable
    {
        public readonly int graphID;
        public Vector2 SourceNode { get; set; }
        
    
        private int totalCellCount;
        private int totalAirCount;
        
        public Dictionary<Vector2, AirCell> cells = new Dictionary<Vector2, AirCell>();
        public Dictionary<Vector2, List<Vector2>> undirectedEdges = new Dictionary<Vector2, List<Vector2>>();
        
        public AirspaceGraph(int graphId, Vector2 sourceNode, int sourceAir = 1)
        {
            Debug.Log($"Created Airspace Graph {graphID} from {sourceNode} with amount = {sourceAir}");
            graphID = graphId;
            this.SourceNode = sourceNode;
            totalAirCount = sourceAir = Mathf.Clamp(sourceAir, 0, 16);
            totalCellCount = 1;
            cells.Add(sourceNode, new AirCell(sourceNode, sourceAir));
        }

        public void SetAirspaceCell(Vector2 source, int currentGas)
        {
            Debug.Log($"Airspace Graph {graphID} SetAirspaceCell({source}, {currentGas})");
            if (!cells.ContainsKey(source))
            {
                cells.Add(source, new AirCell(source, currentGas));
                totalCellCount++;
                totalAirCount += currentGas;
            }
            else
            {
                totalAirCount -= cells[source].air;
                totalAirCount += (cells[source].air = Godot.Mathf.Clamp(currentGas, 0, 16));
            }
        }

        public override string ToString()
        {
            if (totalCellCount <= 0)
            {
                return "Airspace Graph Error: totalCellCount=0";
            }
            return $"Airspace Graph #{graphID}\t Source:{SourceNode}\n\tTotal Space: {totalCellCount}\n\tTotal Air: {totalAirCount}\n\tTarget Density = {totalAirCount/totalCellCount}\n";
        }

        public void Dispose()
        {
            Debug.Log($"Airspace Graph {graphID} Dispose");
            foreach (var airCell in cells)
            {
                GasStuff.trees.Remove(airCell.Key);
            }
            GasStuff.graphs.Remove(this);
        }
        
        public class AirCell
        {
            internal Vector2 position { get; set; }
            internal int air { get; set; }

            public AirCell(Vector2 position, int air)
            {
                this.position = position;
                this.air = air;
            }
        }

    }
} 