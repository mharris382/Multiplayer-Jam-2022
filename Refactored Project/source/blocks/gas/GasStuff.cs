using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.blocks.gas;
using Godot;

public static class GasStuff
{
    private static Dictionary<GridDirections, Vector2> directionVectorLookup;
    static GasStuff()
    {
        directionVectorLookup = new Dictionary<GridDirections, Vector2>();
        directionVectorLookup.Add(GridDirections.RIGHT, Vector2.Right);
        directionVectorLookup.Add(GridDirections.UP, Vector2.Up);
        directionVectorLookup.Add(GridDirections.DOWN, Vector2.Down);
        directionVectorLookup.Add(GridDirections.LEFT, Vector2.Left);
        Sources = new List<SteamSource>();
    }

    public static GasTilemap GasTilemap { get; set; }

    public static SolidBlockTilemap BlockTilemap { get; set; }


    public static List<SteamSource> Sources { get; }


    private static bool HasTileMapAssignments => BlockTilemap != null && GasTilemap != null;


    /// <summary>
    /// async utility method that can be used to delay gas simulation functionality until we are sure that all
    /// external dependencies have been assigned.  Additionally has a timeout in case where the dependencies
    /// will never be assigned due to an error.     
    /// </summary>
    /// <param name="waitSeconds">how long should we wait for dependencies to be assigned</param>
    /// <param name="onError">callback triggered if waitSeconds is reached and still have unassigned dependencies</param>
    public static async Task WaitForAssignments(float waitSeconds = 5, Action onError = null)
    {
        await Task.Run(() =>
        {
            bool error = false;
            int cnt = 0;
            var waitMs = waitSeconds * 1000;
            var msPerCheck = 100;
            var totalChecks = waitMs / msPerCheck;
            
            while (!HasTileMapAssignments)
            {
                if (cnt >= totalChecks)
                {
                    error = true;
                    break;
                }
                cnt++;
                Task.Delay(msPerCheck).Wait();
            }
            
            if(error)
                onError?.Invoke();
        });
    }
    
    
    /// <summary>
    /// function to lookup if a gas cell is blocked by a solid block 
    /// </summary>
    /// <param name="gasCell"></param>
    /// <returns></returns>
    public static bool IsGasCellBlocked(Vector2 gasCell)
    {
        var ws = GasTilemap.MapToWorld(gasCell);
        var bs = BlockTilemap.WorldToMap(ws);
        return BlockTilemap.GetCellv(bs) != -1;
    }
    
    
    
    /// <summary>
    /// iterator for all gas sources.
    /// This method should allow gas sources to be moved around in the world dynamically.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<(Vector2, int)> GetGasFromSourcesToAddToSystem()
    {
        foreach (var steamSource in Sources)
        {
            var pos = steamSource.Position;
            var gasCoord = GasTilemap.WorldToMap(pos);
            var amount = steamSource.Output;
            yield return (gasCoord, amount);
        }
    }
    
    
    
    #region [Directions Code]

    public static GridDirections GetPossibleBlockedDirections(Vector2Int gasGridPosition)
    {
        var div = GasTilemap.steamTilesPerBlockTile;
        var localX = gasGridPosition.x % div;
        var localY = gasGridPosition.y % div;
        
        var hasLeft = localX == 0;
        var hasRight = localX == div - 1;
        var hasUp = localY == 0;
        var hasDown = localY == div - 1;

        var d1 = hasLeft ? GridDirections.LEFT : 0;
        var d2 = hasRight ? GridDirections.RIGHT : 0;
        var d3 = hasUp ? GridDirections.UP : 0;
        var d4 = hasDown ? GridDirections.DOWN : 0;
        // return (GridDirections)(Convert.ToInt32(localX == 0) | 
        //                         (Convert.ToInt32(localY == 0) << 2) | 
        //                         (Convert.ToInt32(localX == div - 1) << 1) | 
        //                         (Convert.ToInt32(localY == div - 1) << 3));
        return (GridDirections)(d1 | d2 | d3 | d4);
    }

    private static Vector2 GasCoordToBlockCoord(Vector2 gasGridPosition)
    {
        var div = GasTilemap.steamTilesPerBlockTile;
        var blockX = gasGridPosition.x / div;
        var blockY = gasGridPosition.y / div;
        return new Vector2(blockX, blockY);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gasGridPosition"></param>
    /// <param name="directionsToCheck"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static GridDirections GetUnBlockedDirections(Vector2Int gasGridPosition, GridDirections directionsToCheck)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// iterates through the encoded directions in order of (Left, Right, Up, Down) and if found returns the vector representation
    /// </summary>
    /// <param name="directions"></param>
    /// <returns></returns>
    private static IEnumerable<Vector2> GetDirections(GridDirections directions)
    {
        if (directions.HasLeft())
            yield return Vector2.Left;
        if (directions.HasRight())
            yield return Vector2.Right;
        if (directions.HasUp())
            yield return Vector2.Up;
        if (directions.HasDown())
            yield return Vector2.Down;
    }

    /// <summary>
    /// returns the number of discrete directions (currently does not include diagnals) encoded in this int (enum).
    /// </summary>
    /// <param name="directions">encoded set of directions</param>
    /// <returns></returns>
    private static int CountDirections(GridDirections directions)
    {
        int cnt = 0;
        if (directions.HasDown()) cnt++;
        if (directions.HasUp()) cnt++;
        if (directions.HasLeft()) cnt++;
        if (directions.HasRight()) cnt++;
        return cnt;
    }
    private static bool HasLeft(this GridDirections directions) => (directions & GridDirections.LEFT) != 0;
    private static bool HasRight(this GridDirections directions) => (directions & GridDirections.RIGHT) != 0;
    private static bool HasUp(this GridDirections directions) => (directions & GridDirections.UP) != 0;
    private static bool HasDown(this GridDirections directions) => (directions & GridDirections.DOWN) != 0;
    
    private static IEnumerable<Vector2> GetNeighborsFromDirections(Vector2 pos, GridDirections directions)
    {
        if(directions == GridDirections.NONE)
            yield break;
        
    }
    

    #endregion
    
    #region [Move to somewhere else]

    public static Vector2 ToGasSpace(this PositionVector positionVector)
    {
        if (HasTileMapAssignments == false)
        {
            Debug.LogWarning("Cannot Convert Positions with missing tilemap dependencies");
            return positionVector.Position;
        }
        switch (positionVector.Space)
        {
            case PositionVector.LocationSpace.WorldSpace:
                throw new NotImplementedException();
            case PositionVector.LocationSpace.GasSpace:
                return positionVector.Position;
            case PositionVector.LocationSpace.BlockSpace:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public static Vector2 ToBlockSpace(this PositionVector positionVector)
    {
        if (HasTileMapAssignments == false)
        {
            Debug.LogWarning("Cannot Convert Positions with missing tilemap dependencies");
            return positionVector.Position;
        }
        switch (positionVector.Space)
        {
            case PositionVector.LocationSpace.WorldSpace:
                throw new NotImplementedException();
            case PositionVector.LocationSpace.GasSpace:
                throw new NotImplementedException();
            case PositionVector.LocationSpace.BlockSpace:
                return positionVector.Position;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        throw new NotImplementedException();
    }
    public static Vector2 ToWorldSpace(this PositionVector positionVector)
    {
        if (HasTileMapAssignments == false)
        {
            Debug.LogWarning("Cannot Convert Positions with missing tilemap dependencies");
            return positionVector.Position;
        }
        switch (positionVector.Space)
        {
            case PositionVector.LocationSpace.WorldSpace:
                return positionVector.Position;
                break;
            case PositionVector.LocationSpace.GasSpace:
                throw new NotImplementedException();
            case PositionVector.LocationSpace.BlockSpace:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
        throw new NotImplementedException();
    }

    public static PositionVector ToWorldSpacePosition(this PositionVector positionVector) => new PositionVector(positionVector.ToWorldSpace());
    public static PositionVector ToBlockSpacePosition(this PositionVector positionVector) => new PositionVector(positionVector.ToBlockSpace(), PositionVector.LocationSpace.BlockSpace);
    public static PositionVector ToGasSpacePosition(this PositionVector positionVector) => new PositionVector(positionVector.ToGasSpace(), PositionVector.LocationSpace.GasSpace);

    #endregion
    
}
