using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public interface IVectorField
    {
        Vector2Int Get(Vector2Int cell);
        void Set(Vector2Int cell, int x, int y);
        int GetX(Vector2Int cell);
        int GetY(Vector2Int cell);
        void SetX(Vector2Int cell, int x);
        void SetY(Vector2Int cell, int y);
    }


    public class VectorField2 : IVectorField
    {
        private CellDataDictionary<int, int> _field = new CellDataDictionary<int, int>();
        
        public Vector2Int Get(Vector2Int cell)
        {
            var res = _field.GetOrDefault(cell, (0, 0));
            return new Vector2Int(res.Item1, res.Item2);
        }

        
        public void Set(Vector2Int cell, int x, int y) => _field.AddOrChange(cell, (x,y));

        public int GetX(Vector2Int cell) => _field.GetT1Safe(cell);

        public int GetY(Vector2Int cell) => _field.GetT2Safe(cell);

        public void SetX(Vector2Int cell, int x) => _field.SetT1Safe(cell, x);

        public void SetY(Vector2Int cell, int y) => _field.SetT2Safe(cell, y);
    }
}