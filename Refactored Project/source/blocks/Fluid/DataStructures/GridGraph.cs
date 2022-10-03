using System;
using System.Collections.Generic;
using Game.Blocks.Gas;
using Game.core;
using Godot;


namespace Game.Blocks.Fluid
{
    
    public class GridGraph 
    {
        private CellDataDictionary<int> _pressure = new CellDataDictionary<int>();
        private VectorField _velocityField = new VectorField();
        
        private Graph<Vector2Int> _graph;
        private Graph<Vector2Int> _lastGraph;

        private Func<Vector2Int, bool> _blockCheck;

        
        public Graph<Vector2Int> Graph => _graph;
        public Graph<Vector2Int> LastGraph => _lastGraph;

        
        public GridGraph(Func<Vector2Int, bool> blockCheck, 
            CellDataDictionary<int> initialCellStates, 
            GraphType type)
        {
            _blockCheck = blockCheck;
            _graph = new Graph<Vector2Int>(type);
            foreach (var initialCellState in initialCellStates)
            {
                AddCell(initialCellState.Key, initialCellState.Value);
                //_graph.AddVertex(initialCellState.Key);
                //_pressure.AddOrChange(initialCellState.Key, initialCellState.Value);
            }
        }


        public IEnumerable<(Vector2Int, int)> GetNewPressureStates()
        {
            foreach (var p in _pressure)
            {
                yield return (p.Key, p.Value);
            }
        }
        public void AddCell(Vector2Int cell, int gas)
        {
            if (!_graph.ContainsVertex(cell))
            {
                _graph.AddVertex(cell);
                
            }

            if (gas == 0)
                _velocityField.SetVelocity(cell, Vector2Int.Zero);
            
            _pressure.AddOrChange(cell, gas);
            
            CheckNeighbor(Vector2Int.U);
            CheckNeighbor(Vector2Int.D);
            CheckNeighbor(Vector2Int.R);
            CheckNeighbor(Vector2Int.L);
            
            void CheckNeighbor(Vector2Int offset)
            {
                var neighbor = cell + offset;
                var exists = _graph.ContainsVertex(neighbor);
                if (exists)
                {
                    UpdateEdge(neighbor);
                }
            }

            void UpdateEdge(Vector2Int neighbor)
            {
                var neighborGas = _pressure[neighbor];
                if (neighborGas == gas)
                {
                    _graph.AddEdge(cell, neighbor, 0);
                    _graph.AddEdge(neighbor, cell, 0);
                }
                else
                {
                    if (neighborGas > gas)//edge points from neighbor to cell
                    {
                        if (!_graph.HasEdge(neighbor, cell))
                            _graph.AddEdge(neighbor, cell, (neighborGas - gas));
                        _graph.RemoveEdge(cell, neighbor);
                    }
                    else //edge points from cell to neighbor
                    {
                        if (!_graph.HasEdge(cell, neighbor)) 
                            _graph.AddEdge(cell, neighbor, (gas - neighborGas));
                        _graph.RemoveEdge(neighbor, cell);
                    }
                }
            }
        }

        public void StartUpdate(IEnumerable<(Vector2Int, int)> states)
        {
            _lastGraph = _graph;
            _graph = new Graph<Vector2Int>(GraphType.DIRECTED_WEIGHTED);
            
            foreach (var valueTuple in states)
            {
                var cell = valueTuple.Item1;
                var pressure = valueTuple.Item2;
                AddCell(cell, pressure);
            }
        }

       
    }
    
    
    
    
}