using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// DFS's frontier is just a simple stack
    /// </summary>
    class DFSFrontierList : IFrontier
    {
        Stack<IBranch> list = new Stack<IBranch>();

        public void Add(IBranch branch)
        {
            list.Push(branch);
        }

        public IBranch GetNext()
        {
            return list.Pop();
        }
    }
}
