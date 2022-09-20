using Godot;

namespace Game.Blocks.TileMapCore
{
    public class BlockLanguageAdapter : Node
    {
        private const string PATH_TO_GD_ADAPTER = "res://source/Blocks/TileMapCore/BlockLanguageAdapter.gd";
        
        public override void _Ready()
        {
            GDScript adapterScript = (GDScript)GD.Load(PATH_TO_GD_ADAPTER);
            base._Ready();
        }
        
        
    }

    public class BlockData
    {
        public int blockID;
        
        

        public BlockData(Godot.Resource gdInst)
        {
            
        }
    }
}