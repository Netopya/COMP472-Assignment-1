using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    class SimpleBranch : IBranch
    {
        private IBranch parent;
        private INode leaf;

        public SimpleBranch(INode leaf, IBranch parent)
        {
            this.leaf = leaf;
            this.parent = parent;
        }

        // Recursively add the cost of reaching the current leaf from the child 
        // until we reach the root
        public int getCost()
        {
            if(parent == null)
            {
                return 0;
            }

            // Ask the parent how much is the cost to get to the current child
            int costHere = parent.getLeaf().getOperations()[leaf];
            return costHere + parent.getCost();
        }

        public INode getLeaf()
        {
            return leaf;
        }

        public IBranch getParent()
        {
            return parent;
        }

        // Recursively print the path to get from the root
        public string printPath()
        {
            if (parent == null)
            {
                return leaf.getName();
            }

            return parent.printPath() + " > " + leaf.getName();
        }
    }
}
