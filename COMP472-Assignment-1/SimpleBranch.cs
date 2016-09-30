﻿using System;
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

        public int getCost()
        {
            if(parent == null)
            {
                return 0;
            }

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

        public string printPath()
        {
            if (parent == null)
            {
                return leaf.getName();
            }

            return parent.printPath() + " > " + leaf.getName();
        }

        public int CompareTo(object obj)
        {
            return getCost() - ((IBranch)obj).getCost();
        }
    }
}
