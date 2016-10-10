using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    // A branch interface to represent the link between a node and its parent in
    // a tree datastructure
    interface IBranch
    {
        IBranch getParent();
        INode getLeaf();
        int getCost();
        string printPath();
    }
}
