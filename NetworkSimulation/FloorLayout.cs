using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class FloorLayout
    {
        [JsonProperty("maxX")]
        private int maxX;

        [JsonProperty("maxY")]
        private int maxY;

        public FloorLayout(int maxX, int maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;
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
    }
}
