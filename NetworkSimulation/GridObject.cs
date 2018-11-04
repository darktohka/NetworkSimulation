using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class GridObject
    {
        
        [JsonProperty("floor")]
        private int floor;

        [JsonProperty("x")]
        private int x;

        [JsonProperty("y")]
        private int y;

        public bool isWall;

        public GridObject(int floor, int x, int y)
        { 
            this.floor = floor;
            this.x = x;
            this.y = y;
            tiles[x, y] = 1;
        }
        
        public int GetFloor()
        {
            return floor;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public void SetFloor(int floor)
        {
            this.floor = floor;
        }

        public void SetX(int x)
        {
            this.x = x;
        }

        public void SetY(int y)
        {
            this.y = y;
        }
    }
}
