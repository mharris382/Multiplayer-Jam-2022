using System;
using System.Collections.Generic;
using Godot;

namespace Game.Gas
{
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

		public void _Process()
		{
			
			
			
		}

		public void ExecuteGasIteration()
		{
			if (!IsSimulatorValid())
				return;

			GasSimulation.DoPreUpdate();
			
			var suppliers = GasSimulation.Suppliers;
			if (suppliers.Count > 0)
			{
				GD.Print($"Number of supplies = {suppliers.Count}");
				foreach (var kvp in suppliers)
				{
					var supplier = kvp.Key;
					var location = kvp.Value;
				}
			}
			
			
			GasSimulation.DoPostUpdate();
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
