using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    interface INode
    {
        string getName();
        Dictionary<INode, int> getOperations();
        int getHeuristic();
        bool getEquals(INode other);
    }
}
