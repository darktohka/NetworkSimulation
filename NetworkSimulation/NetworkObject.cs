using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NetworkSimulation
{
    public class NetworkObject : GridObject
    {
        //BILL MADE THESE
        private float noise;
        //THESE ARE DISYER'S
        [JsonProperty("objectType")]
        private ObjectType objectType;
        [JsonProperty("objectId")]
        private int objectId;
        [JsonProperty("name")]
        private string name;
        [JsonProperty("ipAddress")]//USER INPUT
        private string ipAddress;
        [JsonProperty("uploadMbps")]//USER INPUT
        private double uploadMbps;
        [JsonProperty("downloadMbps")]//USER INPUT
        private double downloadMbps;
        [JsonProperty("throttledUploadMbps")]
        private double throttledUploadMbps;
        [JsonProperty("throttledDownloadMbps")]
        private double throttledDownloadMbps;
        [JsonProperty("avgPingRate")]
        private int avgPingRate;
        [JsonProperty("packetLossChance")]
        private int packetLossChance;
        [JsonProperty("maxConnections")]//USER INPUT OR SET
        private int maxConnections;

        // Computers
        [JsonProperty("actions")]
        private List<Action> actions;
        [JsonProperty("computerType")]
        private ComputerType computerType;
        [JsonProperty("wifiEnabled")]
        private bool wifiEnabled;

        // Routers and wifi extenders
        [JsonProperty("wifiRange")]
        private double wifiRange;
        [JsonProperty("subnet")]
        private string subnet;

        // Temporary variables
        public ConnectionState connectionState;
        public double wifiDropoff = 1.0D;

        public NetworkObject(ObjectType objectType, int objectId, string name, int floor, int x, int y, string ipAddress, double uploadMbps, double downloadMbps, double throttledUploadMbps, double throttledDownloadMbps, int avgPingRate, int packetLossChance, int maxConnections, List<Action> actions, ComputerType computerType, bool wifiEnabled, double wifiRange, string subnet) : base(floor , x, y)
        {//MOST ARE USER INPUT THRU UI===WAKE UP DISYER
            this.objectType = objectType;
            this.objectId = objectId;
            this.name = name;
            this.ipAddress = ipAddress;
            this.uploadMbps = uploadMbps;
            this.downloadMbps = downloadMbps;
            this.throttledUploadMbps = throttledUploadMbps;
            this.throttledDownloadMbps = throttledDownloadMbps;
            this.avgPingRate = avgPingRate;
            this.packetLossChance = packetLossChance;
            this.maxConnections = maxConnections;
            this.actions = actions;
            this.computerType = computerType;
            this.wifiEnabled = wifiEnabled;
            this.wifiRange = wifiRange;
            this.subnet = subnet;
        }

        // WARNING!!! CALL THIS IF ANYTHING CHANGES!!
        public void RecalculateTemporary()
        {
            wifiDropoff = 1.0D;
            connectionState = RecalcConnectionState();
        }

        public NetworkObject GetAssociatedRouter()
        {
            if (!wifiEnabled || (objectType != ObjectType.COMPUTER && objectType != ObjectType.WIFI_EXTENDER))
            {
                // No WIFI capability and not connected to any ethernet.
                return null;
            }

            foreach (NetworkObject obj in Settings.GetSingleton().GetObjects())
            {
                if (obj.GetObjectType() == ObjectType.ROUTER || obj.GetObjectType() == ObjectType.WIFI_EXTENDER)
                {
                    if (DistanceTo(obj) <= obj.GetWifiRange())
                    {
                        wifiDropoff = 1.0D / DistanceTo(obj);
                        return obj;
                    }
                }
            }

            return null;
        }

        public List<int> GetGraphTrace()
        {
            List<int> fWalk = new List<int>();
            fWalk.Add(GetObjectId());

            if (GetObjectType() == ObjectType.MODEM)
            {
                return fWalk;
            }

            Queue<GraphWalk> walks = new Queue<GraphWalk>();
            walks.Enqueue(new GraphWalk(GetObjectId(), fWalk));

            while (walks.Count > 0)
            {
                GraphWalk walk = walks.Dequeue();
                int currentId = walk.currentId;
                List<NetworkObject> objects = Settings.GetSingleton().GetConnectedObjects(currentId);

                foreach (NetworkObject obj in objects)
                {
                    ObjectType type = obj.GetObjectType();

                    if (type == ObjectType.MODEM)
                    {
                        List<int> newList = new List<int>(walk.history);
                        newList.Add(currentId);
                        return newList;
                    }

                    if (type == ObjectType.HUB || type == ObjectType.SWITCH || type == ObjectType.POWERLINE || type == ObjectType.ROUTER)
                    {
                        List<int> newList = new List<int>(walk.history);
                        newList.Add(currentId);
                        walks.Enqueue(new GraphWalk(GetObjectId(), newList));
                    }
                }
            }

            return fWalk;
        }

        public ConnectionState RecalcConnectionState()
        {
            // If connected to a modem, we have internet
            if (objectType == ObjectType.MODEM)
            {
                connectionState = ConnectionState.CONNECTED_CABLE;
                return connectionState;
            }

            Queue<int> visitIds = new Queue<int>();
            visitIds.Enqueue(GetObjectId());

            while (visitIds.Count > 0)
            {
                int currentId = visitIds.Dequeue();
                List<NetworkObject> objects = Settings.GetSingleton().GetConnectedObjects(currentId);

                foreach (NetworkObject obj in objects) {
                    ObjectType type = obj.GetObjectType();

                    if (type == ObjectType.MODEM)
                    {
                        connectionState = ConnectionState.CONNECTED_CABLE;
                        return connectionState;
                    }

                    if (type == ObjectType.HUB || type == ObjectType.SWITCH || type == ObjectType.POWERLINE || type == ObjectType.ROUTER)
                    {
                        if (type == ObjectType.POWERLINE)
                        {
                            // For powerlines, we must check if the powerline is connected to another powerline!
                            List<NetworkObject> powerlineObjs = Settings.GetSingleton().GetConnectedObjects(obj.GetObjectId());
                            bool hasPowerline = false;

                            foreach (NetworkObject powerlineObj in powerlineObjs)
                            {
                                if (powerlineObj.GetObjectType() == ObjectType.POWERLINE)
                                {
                                    hasPowerline = true;
                                    break;
                                }
                            }

                            if (!hasPowerline)
                            {
                                continue;
                            }
                        }

                        visitIds.Enqueue(obj.GetObjectId());
                    }
                }
            }

            // If there is an associated router, we are connected through WiFi
            if (GetAssociatedRouter() != null)
            {
                connectionState = ConnectionState.CONNECTED_WIFI;
                return connectionState;
            }

            // Disconnected
            connectionState = ConnectionState.DISCONNECTED;
            return connectionState;
        }

        public bool IsConnectedToInternet()
        {
            return connectionState != ConnectionState.DISCONNECTED;
        }

        public double DistanceTo(NetworkObject other)//BILL MADE THIS
        {
            return Math.Sqrt(Math.Pow(other.GetX() - GetX(), 2) + Math.Pow(other.GetY() - GetY(), 2) + Math.Pow(other.GetFloor() - GetFloor(), 2));
        }
        public ObjectType GetObjectType()
        {
            return objectType;
        }

        public void SetObjectType(ObjectType objectType)
        {
            this.objectType = objectType;
        }

        public int GetObjectId()
        {
            return objectId;
        }

        public string GetIpAddress()
        {
            return ipAddress;
        }

        public void SetIpAddress(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        private double GetTrueMbps(double realMbps, double throttledMbps)
        {
            double mbps = throttledMbps;

            if (throttledMbps <= 0)
            {
                mbps = realMbps;
 
                if (GetObjectType() == ObjectType.COMPUTER)
                {
                    mbps *= 0.9;
                }
            }

            if (connectionState == ConnectionState.CONNECTED_WIFI)
            {
                mbps *= wifiDropoff;
            }

            return mbps;
        }

        public double GetTrueUploadMbps()
        {
            return GetTrueMbps(uploadMbps, throttledUploadMbps);
        }

        public double GetTrueDownloadMbps()
        {
            return GetTrueMbps(downloadMbps, throttledDownloadMbps);
        }

        public void SetUploadMbps(double uploadMbps)
        {
            this.uploadMbps = uploadMbps;
        }

        public void SetDownloadMbps(double downloadMbps)
        {
            this.downloadMbps = downloadMbps;
        }

        public void SetThrottledUploadMbps(double throttledUploadMbps)
        {
            this.throttledUploadMbps = throttledUploadMbps;
        }

        public void SetThrottledDownloadMbps(double throttledDownloadMbps)
        {
            this.throttledDownloadMbps = throttledDownloadMbps;
        }

        public double GetThrottledUploadMbps()
        {
            return throttledUploadMbps;
        }

        public double GetThrottledDownloadMbps()
        {
            return throttledDownloadMbps;
        }

        public int GetAvgPingRate()
        {
            return avgPingRate;
        }

        public void SetAvgPingRate(int avgPingRate)
        {
            this.avgPingRate = avgPingRate;
        }

        public int GetFinalPingRate()
        {
            if (connectionState == ConnectionState.DISCONNECTED)
            {
                return GetAvgPingRate();
            } else if (connectionState == ConnectionState.CONNECTED_WIFI)
            {
                return GetAvgPingRate() + GetAssociatedRouter().GetFinalPingRate();
            }

            int pingRate = 0;
            List<int> trace = GetGraphTrace();

            foreach (int objectId in trace)
            {
                NetworkObject obj = Settings.GetSingleton().GetObject(objectId);

                if (obj != null)
                {
                    pingRate += obj.GetAvgPingRate();
                }
            }

            return pingRate;
        }

        public int GetPacketLossChance()
        {
            return packetLossChance;
        }

        public void SetPacketLossChance(int packetLossChance)
        {
            this.packetLossChance = packetLossChance;
        }

        public int GetFinalPacketLossChance()
        {
            if (connectionState == ConnectionState.DISCONNECTED)
            {
                return GetPacketLossChance();
            } else if (connectionState == ConnectionState.CONNECTED_WIFI)
            {
                return (GetPacketLossChance() + GetAssociatedRouter().GetFinalPacketLossChance()) / 2;
            }

            int allPacketLossChance = 0;
            int packetLossNum = 0;
            List<int> trace = GetGraphTrace();

            foreach (int objectId in trace)
            {
                NetworkObject obj = Settings.GetSingleton().GetObject(objectId);

                if (obj != null)
                {
                    if (obj.GetPacketLossChance() >= 100)
                    {
                        return 100;
                    }

                    packetLossChance += obj.GetPacketLossChance();
                    packetLossNum += 1;
                }
            }

            if (packetLossNum == 0)
            {
                return 0;
            }
            else
            {
                return (int) Math.Floor((double) allPacketLossChance / packetLossNum);
            }
        }

        public int GetMaxConnections()
        {
            return maxConnections;
        }

        public void SetMaxConnections(int maxConnections)
        {
            this.maxConnections = maxConnections;
        }

        public int GetConnectionCount()
        {
            return Settings.GetSingleton().GetConnectedObjects(GetObjectId()).Count;
        }

        public List<Action> GetActions()
        {
            return actions;
        }

        public void AddAction(Action action)
        {
            if (!actions.Contains(action))
            {
                actions.Add(action);
            }
        }

        public void RemoveAction(Action action)
        {
            if (actions.Contains(action))
            {
                actions.Remove(action);
            }
        }

        public double GetUploadMbpsUsage()
        {
            double mbps = 0;

            foreach (Action action in actions)
            {
                mbps += action.GetDeltaUp();
            }

            return mbps;
        }

        public double GetDownloadMbpsUsage()
        {
            double mbps = 0;

            foreach (Action action in actions)
            {
                mbps += action.GetDeltaDown();
            }

            return mbps;
        }
        
        // This returns the total upload mbps usage for the ENTIRE subgraph
        public MbpsUsage GetTotalUploadMbpsUsage()
        {
            if (connectionState == ConnectionState.DISCONNECTED)
            {
                return new MbpsUsage(GetUploadMbpsUsage(), 0.0);
            } else if (connectionState == ConnectionState.CONNECTED_WIFI)
            {
                MbpsUsage usage = GetAssociatedRouter().GetTotalUploadMbpsUsage();
                usage.currentUsage += GetUploadMbpsUsage();
                return usage;
            }

            double totalMbps = 0.0;
            List<int> trace = GetGraphTrace();

            foreach (int objectId in trace)
            {
                NetworkObject obj = Settings.GetSingleton().GetObject(objectId);

                if (obj != null)
                {
                    totalMbps += obj.GetUploadMbpsUsage();
                }
            }

            double maxMbps = Settings.GetSingleton().GetObject(trace[trace.Count - 1]).GetTrueUploadMbps();
            return new MbpsUsage(totalMbps, maxMbps);
        }
   
        // This returns the total download mbps usage for the ENTIRE subgraph
        public MbpsUsage GetTotalDownloadMbpsUsage()
        {
            if (connectionState == ConnectionState.DISCONNECTED)
            {
                return new MbpsUsage(GetDownloadMbpsUsage(), 0.0);
            } else if (connectionState == ConnectionState.CONNECTED_WIFI)
            {
                MbpsUsage usage = GetAssociatedRouter().GetTotalDownloadMbpsUsage();
                usage.currentUsage += GetDownloadMbpsUsage();
                return usage;
            }

            double totalMbps = 0.0;
            List<int> trace = GetGraphTrace();

            foreach (int objectId in trace)
            {
                NetworkObject obj = Settings.GetSingleton().GetObject(objectId);

                if (obj != null)
                {
                    totalMbps += obj.GetDownloadMbpsUsage();
                }
            }

            double maxMbps = Settings.GetSingleton().GetObject(trace[trace.Count - 1]).GetTrueDownloadMbps();
            return new MbpsUsage(totalMbps, maxMbps);
        }

        public ComputerType GetComputerType()
        {
            return computerType;
        }

        public void SetComputerType(ComputerType computerType)
        {
            this.computerType = computerType;
        }

        public bool GetWifiEnabled()
        {
            return wifiEnabled;
        }

        public void SetWifiEnabled(bool wifiEnabled)
        {
            this.wifiEnabled = wifiEnabled;
        }

        public double GetWifiRange()
        {
            return wifiRange;
        }

        public void SetWifiRange(double wifiRange)
        {
            this.wifiRange = wifiRange;
        }

        public string GetSubnet()
        {
            if (subnet == null)
            {
                // TODO
                // Walk graph and find subnet
                // For example: computers, power lines, etc...
                return "unknown";
            }

            return subnet;
        }

        public void SetSubnet(string subnet)
        {
            this.subnet = subnet;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void RemoveObject()
        {
            Settings.GetSingleton().RemoveObject(this);
        }

        public Bitmap GetImage()
        {
            switch (GetObjectType())
            {
                case ObjectType.COMPUTER:
                    return Properties.Resources.computer;
                case ObjectType.HUB:
                    return Properties.Resources.hub;
                case ObjectType.MODEM:
                    return Properties.Resources.modem;
                case ObjectType.POWERLINE:
                    return Properties.Resources.powerline;
                case ObjectType.ROUTER:
                    return Properties.Resources.router;
                case ObjectType.SWITCH:
                    return Properties.Resources._switch;
                case ObjectType.WIFI_EXTENDER:
                    return Properties.Resources.wifi;
                default:
                    return Properties.Resources.empty;
            }
        }
    }
}
