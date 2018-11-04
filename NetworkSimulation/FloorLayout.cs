using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class FloorLayout
    {
        public int[,,] tiles;
        [JsonProperty("maxX")]
        private int maxX;

        [JsonProperty("maxY")]
        private int maxY;

        public FloorLayout(int floor, int maxX, int maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;
            tiles = new int[floor, maxX,maxY];
        }
        public void ScaleFloor(int x, int y)//WE NEED TO TAKE INPUT FROM THE USER SCALING IT
        {
            SetMaxX(x);
            SetMaxY(y);
        }
        
        public int GetMaxX()
        {
            return maxX;
        }

        public int GetMaxY()
        {
            return maxY;
        }

        public void SetMaxX(int maxX)
        {
            this.maxX = maxX;
        }

        public void SetMaxY(int maxY)
        {
            this.maxY = maxY;
        }
        public bool IsOccupied(int x, int y)
        {
            //DETECT IF THERE IS A GRIDOBJECT ON THIS SQUARE
            return tiles[x, y] >0;
            
        }
        public bool IsEnclosed(int x, int y)
        {
            //DETECT IF THERE ARE WALLS ON 4 SIDES
            return false;
        }
    }
}
