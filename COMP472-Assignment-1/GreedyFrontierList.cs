using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    class GreedyFrontierList : IFrontier
    {
        private List<IBranch> list = new List<IBranch>();

        public void Add(IBranch branch)
        {
            List<IBranch> siblings = new List<IBranch>();
            siblings.Add(branch);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                IBranch possibleSibling = list[i];
                if (possibleSibling.getLeaf().getEquals(branch.getLeaf()))
                {
                    siblings.Add(possibleSibling);
                    list.Remove(possibleSibling);
                }
                else
                {
                    break;
                }
            }

            siblings.OrderBy(x => -x.getLeaf().getHeuristic());
            list.AddRange(siblings);
        }

        public IBranch GetNext()
        {
            return list.Last();
        }
    }
}
