using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class NetworkObject : GridObject
    {
        //BILL MADE THESE
        private float noise;
        [JsonProperty("wifiCoefficient")]//KEVIN HAS FORMULA
        private float wifiCoeff;
        [JsonProperty("deltaAction")]
        private double deltaAct;
        //THESE ARE DISYER'S
        [JsonProperty("objectType")]
        private ObjectType objectType;
        [JsonProperty("objectId")]
        private int objectId;
        [JsonProperty("name")]
        private string name;
        [JsonProperty("ipAddress")]
        private string ipAddress;
        [JsonProperty("uploadMbps")]
        private double uploadMbps;
        [JsonProperty("downloadMbps")]
        private double downloadMbps;
        [JsonProperty("throttledUploadMbps")]
        private double throttledUploadMbps;
        [JsonProperty("throttledDownloadMbps")]
        private double throttledDownloadMbps;
        [JsonProperty("avgPingRate")]
        private int avgPingRate;
        [JsonProperty("packetLossChance")]
        private int packetLossChance;
        [JsonProperty("maxConnections")]
        private int maxConnections;

        // Computers
        [JsonProperty("uploadMbpsUsage")]
        private double uploadMbpsUsage;
        [JsonProperty("downloadMbpsUsage")]
        private double downloadMbpsUsage;
        [JsonProperty("computerType")]
        private ComputerType computerType;
        [JsonProperty("wifiEnabled")]
        private bool wifiEnabled;

        // Routers and wifi extenders
        [JsonProperty("wifiRange")]
        private double wifiRange;
        [JsonProperty("subnet")]
        private string subnet;

        public NetworkObject(ObjectType objectType, int objectId, string name, int floor, int x, int y, string ipAddress, double uploadMbps, double downloadMbps, double throttledUploadMbps, double throttledDownloadMbps, int avgPingRate, int packetLossChance, int maxConnections, double uploadMbpsUsage, double downloadMbpsUsage, ComputerType computerType, bool wifiEnabled, double wifiRange, string subnet) : base(floor , x, y)
        {
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
            this.uploadMbpsUsage = uploadMbpsUsage;
            this.downloadMbpsUsage = downloadMbpsUsage;
            this.computerType = computerType;
            this.wifiEnabled = wifiEnabled;
            this.wifiRange = wifiRange;
            this.subnet = subnet;
        }

        public bool IsConnectedToInternet()
        {
            // TODO
            // Walk the graph, and see if we are connected to a modem.
            UpdateStrength(); //UPDATE MBPS WHEN CONNECT - WE NEED TO FIGURE OUT EACH ACTION
            // If not connected to a modem through the graph, check every router and wifi dropoff here!!!
            if (objectType == ObjectType.MODEM)
            {
                return true;
            }
            foreach(NetworkObject obj in Settings.GetSingleton().GetObjects())//BILL WALKED THE GRAPH!
            {
                if (DistanceTo(obj) > obj.GetWifiRange()) return true;
            }
            // Walk the graph now!
            if (!wifiEnabled)
            {
                // No WIFI capability and not connected to any ethernet.
                return false;
            }
            // Check all routers for dropoff!
            return false;
        }
        public double UpStrength()
        {
            return newUpSpeed() * wifiCoeff;//KEVIN HAS FORMULA
        }
        public double DownStrength()
        {
            return newDownSpeed() * wifiCoeff;//KEVIN HAS FORMULA
        }
        public void UpdateStrength()
        {
            throttledUploadMbps = UpStrength();
            throttledDownloadMbps = DownStrength();
        }
        public double newUpSpeed()//BILL MADE THIS
        {
            return uploadMbps * noise + deltaAct;
        }
        public double newDownSpeed()//BILL MADE THIS
        {
            return downloadMbps * noise + deltaAct;
        }

        public double DistanceTo(NetworkObject other)//BILL MADE THIS
        {
            return Math.sqrt(Math.pow(other.GetX() - GetX(), 2) + Math.pow(other.GetY() - GetY(), 2));
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
            if (throttledMbps >= 0)
            {
                return throttledMbps;
            }
            else
            {
                // The real mbps should cut off a bit.
                return realMbps * 0.9;
            }
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
            // TODO
            // Walk the graph, and add all the ping rates together.
            return avgPingRate;
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
            // TODO
            // Walk the graph, and multiply all packet loss chances together.
            // 100% packet loss chance should mean instant 100% return, however.
            return packetLossChance;
        }

        public double GetUploadMbpsUsage()
        {
            return uploadMbpsUsage;
        }

        public void SetUploadMbpsUsage(double uploadMbpsUsage)
        {
            this.uploadMbpsUsage = Math.Min(GetTrueUploadMbps(), uploadMbpsUsage);
        }

        public double GetDownloadMbpsUsage()
        {
            return downloadMbpsUsage;
        }

        public void SetDownloadMbpsUsage(double downloadMbpsUsage)
        {
            this.downloadMbpsUsage = Math.Min(GetTrueDownloadMbps(), downloadMbpsUsage);
        }

        public bool IsOverusingUpload()
        {
            // TODO
            // Walk the graph, and see if we're using too much upload.
            return false;
        }

        public bool IsOverusingDownload()
        {
            // TODO
            // Walk the graph, and see if we're using too much download.
            return false;
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
    }
}
