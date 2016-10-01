using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    class AStarFrontierList : IFrontier
    {
        class AStarComparer : IComparer<IBranch>
        {
            public int Compare(IBranch x, IBranch y)
            {
                return (x.getCost() + x.getLeaf().getHeuristic()) - (y.getCost() + y.getLeaf().getHeuristic());
            }
        }


        SortedSet<IBranch> list = new SortedSet<IBranch>(new AStarComparer());

        public void Add(IBranch branch)
        {
            /*if(list.Any(x => x.getLeaf().getEquals(branch.getLeaf())))
            {
                return;
            }*/

            list.Add(branch);
        }

        public IBranch GetNext()
        {
            IBranch first = list.First();
            

            Console.Write("        Items: ");
            foreach(var item in list)
            {
                Console.Write(item.getLeaf().getName() + ", ");
            }

            Console.WriteLine();

            list.Remove(first);
            return first;
        }
    }

}
