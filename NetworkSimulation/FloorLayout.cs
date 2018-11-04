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
            foreach(GridObject obj in Settings.GetSingleton().GetObjects())
            {
                if (obj.GetX() == x && obj.GetY() == y) return true;
            }return false;
        }
        public bool IsEnclosed(int x, int y)
        {
            //DETECT IF THERE ARE WALLS ON 4 SIDES
            return false;
        }
    }
}
