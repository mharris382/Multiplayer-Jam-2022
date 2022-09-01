using Godot;
using Godot.Collections;

namespace Game.Testing
{
    public class GdTilemap : ITileMap
    {
        private TileMap _tileMap;
        private readonly bool _valid;
        public GdTilemap( TileMap tileMap)
        {
            _tileMap = tileMap;
            _valid = _tileMap.ThisIsNotNull("Tilemap wrapper given null tilemap");
        }

        public GdTilemap(Node owner, string tileMapPath) : this(owner.GetNode<TileMap>(tileMapPath))
        {
            
        }
        
        public void SetCell(Vector2 vector2, int tile)
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return;
            }
            _tileMap.SetCellv(vector2, tile);
        }
        
        public void SetCell(int x, int y, int tile)
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return;
            }
            _tileMap.SetCell(x, y, tile);
        }
        
        public int GetCell(int x, int y)
        {
            if (!_valid) return 0;
            
            return _tileMap.GetCell(x, y);
        }

        public Array<Vector2> GetUsedCells()
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return new Array<Vector2>();
            }
            return new Array<Vector2>(_tileMap.GetUsedCells());
        }

        public Array<Vector2> GetUsedCellsById(int tileId)
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return new Array<Vector2> ();
            }
            var cells = _tileMap.GetUsedCellsById(tileId);
            return new Array<Vector2>(cells);
        }

        public int GetCell(Vector2 location)
        {
            if (!_valid) return -1;
            return _tileMap.GetCellv(location);
        }

        public Vector2 WorldToMap(Vector2 worldSpaceLocation)
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return worldSpaceLocation;
            }
            
            return _tileMap.WorldToMap(worldSpaceLocation);
        }

        public Vector2 MapToWorld(Vector2 gridSpaceLocation)
        {
            if (!_valid)
            {
                GD.PrintErr("Invalid Tilemap");
                return gridSpaceLocation;
            }

            return _tileMap.MapToWorld(gridSpaceLocation);
        }
    }
}