using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class NetworkCable
    {
        [JsonProperty("cableId")]
        private int cableId;

        [JsonProperty("fromId")]
        private int fromId;

        [JsonProperty("toId")]
        private int toId;

        public NetworkCable(int cableId, int fromId, int toId)
        {
            this.cableId = cableId;
            this.fromId = fromId;
            this.toId = toId;
        }

        public bool IsConnectionPossible()
        {
            // TODO
            // Walk the graph and see if we have any bad connections!
            return true;
        }

        public NetworkObject GetFrom()
        {
            return Settings.GetSingleton().GetObject(fromId);
        }

        public NetworkObject GetTo()
        {
            return Settings.GetSingleton().GetObject(toId);
        }

        public int GetCableId()
        {
            return cableId;
        }

        public int GetFromId()
        {
            return fromId;
        }

        public int GetToId()
        {
            return toId;
        }

        public void RemoveCable()
        {
            Settings.GetSingleton().RemoveNetworkCable(this);
        }
    }
}
