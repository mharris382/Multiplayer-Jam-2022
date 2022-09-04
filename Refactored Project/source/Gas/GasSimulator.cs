using System;
using System.Collections.Generic;
using Godot;

namespace Game.Gas
{
	[Obsolete("early prototype refactored to Game.Gas.SimulationCore")]
	public class GasSimulator : Node2D
	{
		private GasTilemap _gasTilemap;
		private TileMap _blockTilemap;
		 

		internal void SetupGasSimulator(GasTilemap gasTilemap, TileMap blockTilemap)
		{
			this._gasTilemap = gasTilemap ?? throw new ArgumentNullException(nameof(gasTilemap));
			this._blockTilemap = blockTilemap ?? throw new ArgumentNullException(nameof(blockTilemap));
		}
		
		public override void _Ready()
		{
			SetupGasSimulator(GetNode("Steam TileMap") as GasTilemap, GetNode<TileMap>("Block TileMap"));
		}
		
		public void ExecuteGasIteration()
		{
			if (!IsSimulatorValid())
				return;

			GasSimulation.DoPreUpdate();
			
			UpdateAirFromGasSources();
			UpdateAirFromGasSinks();
			CalculateAirFlowGraph();
			UpdateAirFromFlowGraph();
			
			GasSimulation.DoPostUpdate();
		}

		private void UpdateAirFromGasSources()
		{
			var suppliers = GasSimulation.Suppliers;
			if (suppliers.Count > 0)
			{
				GD.Print($"Number of supplies = {suppliers.Count}");
				foreach (var kvp in suppliers)
				{
					var supplier = kvp.Key;
					var location = kvp.Value;
					TryExtractFromSupply(supplier, location);
				}
			}
		}

		private void UpdateAirFromGasSinks()
		{
			throw new NotImplementedException();
		}

		private void CalculateAirFlowGraph()
		{
			throw new NotImplementedException();
		}

		private void UpdateAirFromFlowGraph()
		{
			throw new NotImplementedException();
		}

		private void TryExtractFromSupply(ISteamSupply supply, Vector2 location)
		{
			throw new NotImplementedException();
		}
		
		private bool IsSimulatorValid()
		{
			if (_gasTilemap == null || _blockTilemap == null)
			{
				if (_blockTilemap == null)
					GD.PrintErr("Null Block Tilemap");
				if (_gasTilemap == null)
					GD.PrintErr("Null Gas Tilemap");
				return false;
			}

			return true;
		}
	}

	

	public class TileSizeMismatchException : Exception
	{
		public TileSizeMismatchException(TileMap tilemap1, TileMap tileMap2) : base($"sizes of tilemaps don't match:\n{tilemap1.Name}({tilemap1.CellSize})\n{tileMap2.Name}({tileMap2.CellSize})") { }
	}
}
