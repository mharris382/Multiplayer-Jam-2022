using Godot;

namespace Game.Testing
{
    public class GridGraphVisualizer : Node2D
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        
        
        [Export()]
        private NodePath _targetNodePath = "text";

        [Export()]
        private NodePath _visualizerStrategy = "";
        
        [Export()] private string[] states;
        private TileMap _targetMap;
        
        private IGridGraphVisualStrategy _graphVisual;

        private bool _valid = false;
        
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _valid = false;
            _targetMap = GetNode<TileMap>(_targetNodePath);
            if (this.Assert(_targetMap != null, "Node Path not valid"))
                return;
            _graphVisual = GetNode(_visualizerStrategy) as IGridGraphVisualStrategy;
            if(this.Assert(_graphVisual!=null, "No Visualizer Strategy Assigned"))
                   return;
            _valid = true;            
        }

        public void _timeout()
        {
            GD.Print("Timeout");
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

        
    }
}
