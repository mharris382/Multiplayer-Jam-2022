using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public class VectorField
    {
        private CellDataDictionary<int> _xVelocity = new CellDataDictionary<int>();
        private CellDataDictionary<int> _yVelocity = new CellDataDictionary<int>();

        public Vector2Int GetVelocity(Vector2Int cell)
        {
            return new Vector2Int(_xVelocity.GetOrDefault(cell, 0), _yVelocity.GetOrDefault(cell, 0));
        }

        public void SetVelocity(Vector2Int cell, Vector2Int velocity)
        {
            _xVelocity.AddOrChange(cell, velocity.x);
            _yVelocity.AddOrChange(cell, velocity.y);
        }

        public Vector2Int GetLastVelocity(Vector2Int cell)
        {
            return new Vector2Int(_xVelocity.GetOrDefault(cell, 0), _yVelocity.GetOrDefault(cell, 0));
        }
    }
}