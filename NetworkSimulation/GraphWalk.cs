using System;
using System.Collections.Generic;

namespace NetworkSimulation
{
    public class GraphWalk
    {
        public int currentId;
        public List<int> history;

        public GraphWalk(int currentId, List<int> history)
        {
            this.currentId = currentId;
            this.history = history;
        }
    }
}
