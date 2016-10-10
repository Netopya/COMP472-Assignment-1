using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// This "Greedy" frontier is similar to DFS however when a node is expanded and
    ///     its children are added to the list, these children are sorted so that the
    ///     best one is retrieved next. The comparison only occurs amongst the siblings
    ///     and not the entire frontier
    /// </summary>
    class GreedyFrontierList : IFrontier
    {
        // We use a simple list but intelligently places the items on add
        private List<IBranch> list = new List<IBranch>();

        public void Add(IBranch branch)
        {
            List<IBranch> siblings = new List<IBranch>();
            siblings.Add(branch);
            
            // Go through the last items to find siblings of the branch we are adding,
            // save them and remove them from the main list
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

            // Order the siblings we have found based on their heuristic
            siblings.OrderBy(x => -x.getLeaf().getHeuristic());
            
            // Add the siblings back to the list
            list.AddRange(siblings);
        }

        public IBranch GetNext()
        {
            var next = list.Last();
            list.Remove(next);
            return next;
        }
    }
}
