using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    class SimpleNode : INode
    {
        private string name;
        private Dictionary<INode, int> operations = new Dictionary<INode, int>();
        private int heuristic;

        public SimpleNode(int heuristic, string name)
        {
            this.heuristic = heuristic;
            this.name = name;
        }

        public void MakeLink(int cost, INode child)
        {
            operations.Add(child, cost);
        }

        public string getName()
        {
            return name;
        }

        public Dictionary<INode, int> getOperations()
        {
            return operations;
        }

        public int getHeuristic()
        {
            return heuristic;
        }

        public bool getEquals(INode other)
        {
            return this == other;
        }
    }
}
