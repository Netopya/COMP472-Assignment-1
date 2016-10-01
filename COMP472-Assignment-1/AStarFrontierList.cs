using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace COMP472_Assignment_1
{
    class AStarFrontierList : IFrontier
    {
        class AStarComparer : IComparer<IBranch>
        {
            public int Compare(IBranch x, IBranch y)
            {
                int compareValue = (x.getCost() + x.getLeaf().getHeuristic()) - (y.getCost() + y.getLeaf().getHeuristic());

                // Don't allow 0 values since items with the same sort value get rejected by the SortedSet
                //return compareValue == 0 ? -1 : compareValue;
                return compareValue;
            }
        }

        //SortedSet<IBranch> list = new SortedSet<IBranch>(new AStarComparer());
        //List<IBranch> list = new List<IBranch>();
        OrderedBag<IBranch> list = new OrderedBag<IBranch>(new AStarComparer());

        public void Add(IBranch branch)
        {
            /*if(list.Any(x => x.getLeaf().getEquals(branch.getLeaf())))
            {
                return;
            }*/

            IBranch contender = list.AsEnumerable().FirstOrDefault(x => x.getLeaf().getEquals(branch.getLeaf()));

            

            if(contender != null)
            {
                if(contender.getCost() > branch.getCost())
                {
                    list.Remove(contender);
                }
                else
                {
                    return;
                }
            }

            list.Add(branch);
            //var comparer = new AStarComparer();
            //list.Sort((x, y) => comparer.Compare(x, y));
        }

        public IBranch GetNext()
        {
            //Console.WriteLine("        Original Items: " + string.Join(", ", list.AsEnumerable().Select(x => x.getLeaf().getName())));
            IBranch first = list.RemoveFirst();
            

            //Console.WriteLine("        Popping: " + first.getLeaf().getName());

            //Console.WriteLine("        New Items: " + string.Join(", ", list.AsEnumerable().Select(x => x.getLeaf().getName())));

            return first;
        }
    }

}
