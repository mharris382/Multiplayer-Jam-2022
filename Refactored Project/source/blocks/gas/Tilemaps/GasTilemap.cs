using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Game.Blocks.Fluid;
using Game.blocks.gas;
using Game.Blocks.Solids;
using Game.core;
using Godot;
using Godot.Collections;
using Debug = Game.core.Debug;

namespace Game.Blocks.Gas.Tilemaps
{
    public class GasTilemap : TileMap, IFluid
    {
        private const int MAX_STEAM_VALUE = 16;


        [Export(hint: PropertyHint.ExpRange, hintString: "1,32,2")]
        public int steamTilesPerBlockTile = 4;

        [Export()] public NodePath pathToBlockTilemap = "./Block TileMap";
        

        private System.Collections.Generic.Dictionary<Vector2Int, int> _knownGasStates = new CellDataDictionary<int>();


        private Vector2Int[] _blockToGasOffsets;

        public void ClearBlockCell(Vector2Int blockCell)
        {
            var gasCell = blockCell * steamTilesPerBlockTile;
            foreach (var blockToGasOffset in _blockToGasOffsets)
            {
                var c = gasCell + blockToGasOffset;
                SetSteam(c, 0);
                _knownGasStates.Remove(c);
            }
        }

        public void ReadGasStatesFromTileMap()
        {
            _knownGasStates.Clear();
            
            int foundCells = 0;
            int foundGas = 0;
            
            foreach (var cellState in GetUseCellStates())
            {
                _knownGasStates.AddOrReplace(cellState.cell, cellState.pressure);
                foundCells++;
                foundGas += cellState.pressure;
            }
        }
        
        public override void _Ready()
        {
            OnCellClearedOfGas += i =>
            {
                _knownGasStates.Remove(i);
            };
            _blockToGasOffsets = new Vector2Int[steamTilesPerBlockTile * steamTilesPerBlockTile];
            int cnt = 0;
            for (int i = 0; i < steamTilesPerBlockTile; i++)
            {
                for (int j = 0; j < steamTilesPerBlockTile; j++)
                {
                    _blockToGasOffsets[cnt++] = new Vector2Int(i, j);
                }
            }
            GasStuff.GasTilemap = this;
            
        }


        /// <summary>
        /// gets the steam value mapped to the given tileset tile index
        /// </summary>
        /// <param name="tileIndex">0-15</param>
        /// <returns>steam value [1-16]</returns>
        private static int TileIdToSteam(int tileIndex) => tileIndex + 1;


        /// <summary> gets the tileset tile index for a given steam value </summary>
        /// <param name="steamValue">should range from 1-16</param>
        /// <returns>tile set id [0-15]</returns>
        private static int SteamToTileId(int steamValue) => steamValue - 1;



        /// <summary> adds gas to the tilemap from a gas source </summary>
        /// <returns>amount of gas added</returns>
        public int AddGasFromSource(Vector2 cell, int gasAmount)
        {
            return ModifySteam(cell, gasAmount);
        }

        public IEnumerable<(Vector2 cell, int gas)> GetGasCells()
        {
            for (int i = 0; i < 16; i++)
            {
                int s = 16 - i;
                foreach (var cell in GetUsedCellsById(s-1))
                {
                    yield return (cell, s);
                }
            }
        }

        public void SyncToSim(ISimulationUpdater updater)
        {
            updater.PreSimulationStep += UpdaterOnPreSimulationStep;
            
        }


        private void UpdaterOnPreSimulationStep(int obj)
        {
            ReadGasStatesFromTileMap();
        }
        
        

        public int ModifySteam(Vector2 tilePosition, int amountToAdd)
        {
            var current = GetSteam(tilePosition);
            if (current + amountToAdd <= MAX_STEAM_VALUE)
            {
                SetSteam(tilePosition, current + amountToAdd);
                return amountToAdd;
            }
        
            var amountAdded = MAX_STEAM_VALUE - current;
            SetSteam(tilePosition, MAX_STEAM_VALUE);
            return amountAdded;
        }

        
        /// <summary>
        /// removes the amount provided there is that much to remove, otherwise removes all the steam
        /// TODO: make an AddSteam version of this method
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="amountToRemove"></param>
        /// <returns>returns the amount of steam that was removed</returns>
        public int RemoveSteam(Vector2 cell, int amountToRemove)
        {
            var amount = Mathf.Abs(amountToRemove); //accept negative input or positive
            var prevSteam = GetSteam(cell);

            if (prevSteam > amount)
            {
                SetSteam(cell, prevSteam - amount);
                return amount;
            }

            SetSteam(cell, 0);
            return prevSteam;
        }


        /// <summary>
        /// tries to transfer the desired amount of steam from one cell to another cell WITH conservation of mass.
        /// Returns true if ANY steam was transferred.
        /// NOTE: the amount transfer will be less than or equal to the transferAmount
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="transferAmount">desired amount to transfer, actual amount may be less</param>
        /// <returns>false only if no steam was moved</returns>
        private bool TransferSteam(Vector2 fromPosition, Vector2 toPosition, ref int transferAmount)
        {
            var fromGas = GetSteam(fromPosition);
            var toGas = GetSteam(toPosition);
            var amountCanAdd = 16 - toGas;
            var amountCanTake = fromGas;
            var amountCanMove = Mathf.Min(amountCanAdd, amountCanTake);
            var amount = Mathf.Min(transferAmount, amountCanMove);
            if (amount > 0)
            {
                var a1 = ModifySteam(fromPosition, -amount);
                ModifySteam(toPosition, amount);
            }

            transferAmount = amount;
            return amount > 0;
        }

        /// <summary>
        /// tries to transfer the given amount from a to b
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public int MoveSteam(Vector2 from, Vector2 to, int amt)
        {
            var fmAmount = from.GetGasAmount();
            var toAmount = to.GetGasAmount();
            var toSpace = 16 - toAmount;
            var toSupply = fmAmount;
            amt = Mathf.Clamp(amt, toSpace, toSupply);
            TransferSteam(from, to, ref amt);
            return amt;
        }

        private void SetSteam(int x, int y, int steamValue)
        {
            steamValue = Mathf.Clamp(steamValue, 0, MAX_STEAM_VALUE);
            var current = GetSteam(x, y);
            if (current != steamValue)
            {
                SetCell(x, y, SteamToTileId(steamValue));
                if (steamValue == 0)
                {
                    var obj = new Vector2Int(x, y);
                    _knownGasStates.Remove(obj);
                    OnCellClearedOfGas?.Invoke(obj);
                }
            }
        }


        public void SetSteam(Vector2 tilePosition, int steamValue) =>
            SetSteam((int)tilePosition.x, (int)tilePosition.y, steamValue);


        private int GetSteam(int tileX, int tileY)
        {
            //TODO: this code will break if a tile index above 16 is used, which could easily happen
            return TileIdToSteam(GetCell(tileX, tileY));
        }

        
        public int GetSteam(Vector2 tilePosition) => GetSteam((int)tilePosition.x, (int)tilePosition.y);

        
        /// <summary>
        /// clears gas from given gas cells.  returns the amount of gas removed from the simulation.
        /// assumes all gas cells are valid
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int ClearCells(IEnumerable<Vector2> cells)
        {
            int cnt = 0;
            foreach (var cell in cells)
            {
                cnt += GetSteam(cell);
                SetCellv(cell, -1);
            }

            return cnt;
        }

        
        public new Array<Vector2> GetUsedCells() => new Array<Vector2>(base.GetUsedCells());
        
        public new Array<Vector2> GetUsedCellsById(int tileId) => new Array<Vector2>(base.GetUsedCellsById(tileId));


        private System.Collections.Generic.Dictionary<Vector2Int, Vector2Int> gasToCellCache =
            new CellDataDictionary<Vector2Int>();

        Vector2Int GetBlockCell(Vector2Int gasCell)
        {
            if (gasToCellCache.TryGetValue(gasCell, out var blockCell))
            {
                return blockCell;
            }
            gasToCellCache.Add(gasCell, BlockMap.ConvertToBlockCell(gasCell));
            return gasToCellCache[gasCell];
        }
        public void WriteCells(IEnumerable<(Vector2Int gasCell, int pressure)> cellsToWrite, CellWriteMode mode)
        {
            System.Collections.Generic.Dictionary<Vector2Int, int> _modifiedCells =
                new System.Collections.Generic.Dictionary<Vector2Int, int>();
            
            int totalCount = 0;
            int totalChanged = 0;

            IEnumerable<(Vector2Int, int)> GetGasCells((Vector2Int gasCell, int gas) t)
            {
                if (BlockMap.IsCellBlocked(BlockMap.ConvertToBlockCell(t.gasCell)))
                {
                    for (int i = 0; i < steamTilesPerBlockTile; i++)
                    {
                        for (int j = 0; j < steamTilesPerBlockTile; j++)
                        {
                            var cell = t.gasCell + new Vector2Int(i, j);
                            yield return (cell, 0);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < steamTilesPerBlockTile; i++)
                    {
                        for (int j = 0; j < steamTilesPerBlockTile; j++)
                        {
                            var cell = t.gasCell + new Vector2Int(i, j);
                            yield return (cell , GetSteam(cell));
                        }
                    }
                }
            }

            var resu = cellsToWrite.ToList();
                //.Where(t => (t.gasCell.x % steamTilesPerBlockTile) == 0 && (t.gasCell.y % steamTilesPerBlockTile) == 0)
                //.SelectMany(GetGasCells).ToList();
            
            foreach (var cell in resu
                         //.AsParallel()
                     )
            {
                
                AddValueTup(cell);
            }
          
            
            
            if (mode == CellWriteMode.OVERWRITE_CLEAR)
            {
                foreach (var knownGasState in _knownGasStates)
                {
                    if (!_modifiedCells.ContainsKey(knownGasState.Key))
                    {
                        SetSteam(knownGasState.Key, 0);
                    }
                }
            }
            if(totalCount > 0)
                Debug.Log($"{totalChanged} cell changes made from {totalCount} given cells");
            
            void AddValueTup((Vector2Int gasCell, int pressure) valueTuple)
            {
                totalCount++;
                var cell = valueTuple.gasCell;
                var gasValue = valueTuple.pressure;
                var prevGasValue = GetSteam(cell.x, cell.y);
                var newGasValue = prevGasValue;
                
                switch (mode)
                {
                    case CellWriteMode.OVERWRITE:
                    case CellWriteMode.OVERWRITE_CLEAR:
                        newGasValue = gasValue;
                        break;
                    case CellWriteMode.BLEND_ADD:
                        newGasValue = gasValue + prevGasValue;
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
                newGasValue = BlockMap.IsCellBlocked(GetBlockCell(cell)) ? 0 : Mathf.Clamp(newGasValue, 0, 16);
                if (prevGasValue != newGasValue)
                {
                    totalChanged++;
                    SetSteam(cell.x, cell.y,   newGasValue);
                    _modifiedCells.Add(cell, newGasValue);
                }
            }

        }

        

        public IBlockMap BlockMap;

        
        public void InjectBlockMap(IBlockMap blockMap)
        {
            BlockMap = blockMap;
            BlockMap.BlockCellAdded += ClearBlockCell;
            BlockMap.BlockCellRemoved += ClearBlockCell;
        }
        public IEnumerable<(Vector2Int cell, int pressure)> GetUseCellStates()
        {
            HashSet<Vector2Int> _found = new HashSet<Vector2Int>();
            for (int i = 0; i < 16; i++)
            {
                foreach (var vec in GetUsedCellsById(i))
                {
                    if (_knownGasStates.ContainsKey(vec))
                    {
                        _knownGasStates[vec] = GetSteam(vec);
                    }
                    else
                    {
                        _knownGasStates.Add(vec, GetSteam(vec));
                    }

                    _found.Add(vec);
                    yield return (new Vector2Int(vec),_knownGasStates[vec]);
                }
            }

            
            Task.Run(() =>
            {
                var keys = _knownGasStates.Keys.ToList();
                foreach (var key in keys)
                {
                    if (_found.Contains(key) == false)
                    {
                        _knownGasStates.Remove(key);
                    }
                }
            });

        }

        public event Action<Vector2Int> OnCellClearedOfGas;
    }
}
