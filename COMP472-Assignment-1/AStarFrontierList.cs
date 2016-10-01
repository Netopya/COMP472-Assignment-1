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


        List<IBranch> list = new List<IBranch>();

        public void Add(IBranch branch)
        {
            /*if(list.Any(x => x.getLeaf().getEquals(branch.getLeaf())))
            {
                return;
            }*/

            list.Add(branch);
            var comparer = new AStarComparer();
            list.Sort((x, y) => comparer.Compare(x, y));
        }

        public IBranch GetNext()
        {
            IBranch first = list.First();


            /*Console.Write("        Items: ");
            foreach(var item in list)
            {
                Console.Write(item.getLeaf().getName() + ", ");
            }

            Console.WriteLine();*/

            list.Remove(first);
            return first;
        }
    }

}
