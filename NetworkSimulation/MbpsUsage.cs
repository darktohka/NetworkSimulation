using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSimulation
{
    public class MbpsUsage
    {
        public double currentUsage;
        public double maxUsage;

        public MbpsUsage(double currentUsage, double maxUsage)
        {
            this.currentUsage = currentUsage;
            this.maxUsage = maxUsage;
        }

        public bool IsOverloaded()
        {
            return currentUsage > maxUsage;
        }
    }
}
