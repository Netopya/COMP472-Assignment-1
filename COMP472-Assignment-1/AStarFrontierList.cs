using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// The A* Star frontier orders each node according to the result
    /// of the function f(n) = cost(n) + heuristic(n)
    /// </summary>
    class AStarFrontierList : IFrontier
    {
        // Class to compare branches in the frontier
        class AStarComparer : IComparer<IBranch>
        {
            public int Compare(IBranch x, IBranch y)
            {
                int compareValue = (x.getCost() + x.getLeaf().getHeuristic()) - (y.getCost() + y.getLeaf().getHeuristic());
                return compareValue;
            }
        }

        // The OrderedBag from PowerCollections sorts items according to the provided IComparer object
        // Such a specialized datastructure was required since built-in ordered datastructures do not allow
        //     multiple items with the same comparison value (which in our case is the heuristic)
        OrderedBag<IBranch> list = new OrderedBag<IBranch>(new AStarComparer());

        public void Add(IBranch branch)
        {
            // Check to see if the branch is already in the collection
            IBranch contender = list.AsEnumerable().FirstOrDefault(x => x.getLeaf().getEquals(branch.getLeaf()));

            if(contender != null)
            {
                if(contender.getCost() > branch.getCost())
                {
                    // If the new branch is better, remove the existing one
                    list.Remove(contender);
                }
                else
                {
                    return;
                }
            }

            list.Add(branch);
        }

        // "Pop" the first item off the OrderedBag
        public IBranch GetNext()
        {
            IBranch first = list.RemoveFirst();

            return first;
        }
    }

}
