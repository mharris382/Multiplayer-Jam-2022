using Game.Blocks.Gas;

namespace Game.Blocks.Fluid
{
    public struct CellSource
    {
        private  Vector2Int position;
        private byte rate;

        public Vector2Int Position => position;
        public int Rate => rate;
        
        public CellSource(Vector2Int position, byte rate)
        {
            this.position = position;
            this.rate = rate;
        }
    }
    
                
            

    
}