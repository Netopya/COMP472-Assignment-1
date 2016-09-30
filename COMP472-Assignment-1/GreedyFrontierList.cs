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
            throw new NotImplementedException();
        }

        public IBranch GetNext()
        {
            throw new NotImplementedException();
        }
    }
}
