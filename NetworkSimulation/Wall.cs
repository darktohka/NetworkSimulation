using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class Wall : GridObject
    {
        public Wall(int floor, int x, int y) : base(floor, x, y)
        {
            isWall = true;
        }
    }
}
