using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class Action
    {
        [JsonProperty("name")]
        private string name;

        [JsonProperty("deltaUp")]
        private int deltaUp;

        [JsonProperty("deltaDown")]
        private int deltaDown;

        public Action(string name, int deltaUp, int deltaDown)
        {
            this.name = name;
            this.deltaUp = deltaUp;
            this.deltaDown = deltaDown;
        }

        public string GetName()
        {
            return name;
        }
        
        public void SetName(string name)
        {
            this.name = name;
        }

        public int GetDeltaUp()
        {
            return deltaUp;
        }

        public void SetDeltaUp(int deltaUp)
        {
            this.deltaUp = deltaUp;
        }

        public int GetDeltaDown()
        {
            return deltaDown;
        }

        public void SetDeltaDown(int deltaDown)
        {
            this.deltaDown = deltaDown;
        }
    }
}