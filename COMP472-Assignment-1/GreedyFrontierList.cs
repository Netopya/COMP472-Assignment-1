using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    class GreedyFrontierList : IFrontier
    {
        
        SortedSet<IBranch> list = new SortedSet<IBranch>();

        public void Add(IBranch branch)
        {
            list.Add(branch);
        }

        public IBranch GetNext()
        {
            IBranch first = list.First();
            list.Remove(first);
            return first;
        }
    }
}
