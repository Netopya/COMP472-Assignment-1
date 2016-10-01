using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    interface IBranch
    {
        IBranch getParent();
        INode getLeaf();
        int getCost();
        string printPath();
    }
}
