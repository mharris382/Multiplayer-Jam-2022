using Godot;

namespace Game.Testing
{
    public interface IGridGraphVisualStrategy
    {
        void Initialize(IReadOnlyTileMap targetMap);
        void VisualizeGrid(ITileMap visualMap);
    }

    public class BlockChecker : Node
    {
        [Export()]
        public NodePath blockTileMapPath = "";
    }
    
    
    public class AirspaceVisualStrategy : Node, IGridGraphVisualStrategy
    {
        private IReadOnlyTileMap _targetMap;
        private bool _invalid = false;
        
        

        public void Initialize(IReadOnlyTileMap targetMap)
        {
            this._targetMap = targetMap;
            if (this.Assert(_targetMap != null, "Target Map Null"))
            {
                _invalid = true;
                return;
            }
            var cells = this._targetMap.GetUsedCells();
            _invalid = false;
        }

        public void VisualizeGrid(ITileMap visualMap)
        {
            if (_invalid || visualMap == null) return;
            
            throw new System.NotImplementedException();
        }
    }
}