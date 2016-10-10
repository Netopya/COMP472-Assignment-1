using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// BFS's frontier is just a simple Queue
    /// </summary>
    class BFSFrontierList : IFrontier
    {
        Queue<IBranch> list = new Queue<IBranch>();

        public void Add(IBranch branch)
        {
            list.Enqueue(branch);
        }

        public IBranch GetNext()
        {
            return list.Dequeue();
        }
    }
}
