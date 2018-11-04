using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetworkSimulation
{
    public class Door : Wall
    {
        public Door(int floor, int x, int y) : base(floor, x, y)
        {
        }
    }
}
