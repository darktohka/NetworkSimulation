using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NetworkSimulation
{
    public class Settings
    {
        private static Settings singleton = null;

        [JsonProperty("objects")]
        private List<NetworkObject> objects = new List<NetworkObject>();

        [JsonProperty("cables")]
        private List<NetworkCable> cables = new List<NetworkCable>();

        [JsonProperty("walls")]
        private List<Wall> walls = new List<Wall>();

        [JsonProperty("floors")]
        private List<FloorLayout> floors = new List<FloorLayout>();

        [JsonProperty("nextObjectId")]
        private int nextObjectId = 0;

        [JsonProperty("nextCableId")]
        private int nextCableId = 0;

        public static Settings GetSingleton()
        {
            return singleton;
        }

        public static void SetSingleton(Settings singleton)
        {
            Settings.singleton = singleton;
        }

        public static void LoadSettings()
        {
            if (!File.Exists("settings.json"))
            {
                singleton = new Settings();
                SaveSettings();
            }

            try
            {
                singleton = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            } catch
            {
                MessageBox.Show("Couldn't load settings!", "Important!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public static void SaveSettings()
        {
            try
            {
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(singleton));
            } catch
            {
                MessageBox.Show("Couldn't save settings!", "Important!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            GetSingleton().ReevaluateObjects();
        }

        public List<NetworkObject> GetObjects()
        {
            return objects;
        }
        
        public void AddObject(NetworkObject obj)
        {
            if (!objects.Contains(obj))
            {
                objects.Add(obj);
            }
        }

        public void RemoveObject(NetworkObject obj)
        {
            if (objects.Contains(obj))
            {
                objects.Remove(obj);
            }
        }

        public NetworkObject GetObject(int objectId)
        {
            foreach (NetworkObject obj in objects)
            {
                if (obj.GetObjectId() == objectId)
                {
                    return obj;
                }
            }

            return null;
        }
        
        public int AllocateObjectId()
        {
            int currentId = nextObjectId;
            nextObjectId += 1;
            return currentId;
        }

        public List<NetworkCable> GetCables()
        {
            return cables;
        }

        public void AddNetworkCable(NetworkCable cable)
        {
            if (!cables.Contains(cable))
            {
                cables.Add(cable);
            }
        }

        public void RemoveNetworkCable(NetworkCable cable)
        {
            if (cables.Contains(cable))
            {
                cables.Remove(cable);
            }
        }

        public int AllocateCableId()
        {
            int currentId = nextCableId;
            nextCableId += 1;
            return currentId;
        }

        public NetworkCable GetNetworkCable(int cableId)
        {
            foreach (NetworkCable cable in cables)
            {
                if (cable.GetCableId() == cableId)
                {
                    return cable;
                }
            }

            return null;
        }

        public List<Wall> GetWalls()
        {
            return walls;
        }

        public void AddWall(Wall wall)
        {
            if (!walls.Contains(wall))
            {
                walls.Add(wall);
            }
        }

        public void RemoveWall(Wall wall)
        {
            if (walls.Contains(wall))
            {
                walls.Remove(wall);
            }
        }

        public void RemoveGridObject(GridObject obj)
        {
            if (obj is NetworkObject)
            {
                RemoveObject((NetworkObject)obj);
            } else if (obj is Wall)
            {
                RemoveWall((Wall)obj);
            }
        }

        public List<FloorLayout> GetFloors()
        {
            return floors;
        }

        public void AddFloor(FloorLayout floor)
        {
            if (!floors.Contains(floor))
            {
                floors.Add(floor);
            }
        }

        public void RemoveFloor(int floorNum)
        {
            if (floorNum < floors.Count)
            {
                floors.RemoveAt(floorNum);
            }

            List<NetworkObject> removeObjs = new List<NetworkObject>();
            List<Wall> removeWalls = new List<Wall>();

            foreach (NetworkObject obj in GetObjects())
            {
                // Remove all objects on this floor
                if (obj.GetFloor() == floorNum)
                {
                    removeObjs.Add(obj);
                }

                // Shift all objects down by one floor
                if (obj.GetFloor() > floorNum)
                {
                    obj.SetFloor(obj.GetFloor() - 1);
                }
            }

            foreach (Wall wall in GetWalls())
            {
                // Remove all walls on this floor
                if (wall.GetFloor() == floorNum)
                {
                    removeWalls.Add(wall);
                }

                // Shift all walls down by one floor
                if (wall.GetFloor() > floorNum)
                {
                    wall.SetFloor(wall.GetFloor() - 1);
                }
            }

            foreach (NetworkObject obj in removeObjs)
            {
                RemoveObject(obj);
            }
            
            foreach (Wall wall in removeWalls)
            {
                RemoveWall(wall);
            }

            // Remove all orphaned cables
            RemoveOrphanedCables();
        }

        public void RemoveOrphanedCables()
        {
            List<NetworkCable> removeCables = new List<NetworkCable>();

            foreach (NetworkCable cable in GetCables())
            {
                if (cable.GetFrom() == null || cable.GetTo() == null)
                {
                    removeCables.Add(cable);
                }
            }

            foreach (NetworkCable cable in removeCables)
            {
                RemoveNetworkCable(cable);
            }

            ReevaluateObjects();
        }

        public void ReevaluateObjects()
        {
            foreach (NetworkObject obj in GetObjects())
            {
                obj.RecalculateTemporary();
            }
        }

        public FloorLayout GetFloor(int floorNum)
        {
            // Starts from 0.
            return floors[floorNum];
        }

        public int GetFloorCount()
        {
            return floors.Count;
        }

        public GridObject GetGridObject(int floor, int x, int y)
        {
            foreach (NetworkObject obj in objects)
            {
                if (obj.GetFloor() == floor && obj.GetX() == x && obj.GetY() == y)
                {
                    return obj;
                }
            }

            foreach (Wall wall in walls)
            {
                if (wall.GetFloor() == floor && wall.GetX() == x && wall.GetY() == y)
                {
                    return wall;
                }
            }

            return null;
        }

        public void RemoveGridObject(int floor, int x, int y)
        {
            GridObject gridObj = GetGridObject(floor, x, y);

            if (gridObj != null)
            {
                if (gridObj is NetworkObject)
                {
                    RemoveObject((NetworkObject)gridObj);
                } else if (gridObj is Wall)
                {
                    RemoveWall((Wall)gridObj);
                }
            }
        }

        public bool IsSpaceEmpty(int floor, int x, int y)
        {
            return GetGridObject(floor, x, y) == null;
        }

        public List<NetworkObject> GetConnectedObjects(int objectId)
        {
            List<NetworkObject> objs = new List<NetworkObject>();

            foreach (NetworkCable cable in cables)
            {
                if (cable.GetFromId() == objectId)
                {
                    objs.Add(cable.GetTo());
                } else if (cable.GetToId() == objectId)
                {
                    objs.Add(cable.GetFrom());
                }
            }

            return objs;
        }
    }
}
