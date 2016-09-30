using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    interface IFrontier
    {
        void Add(IBranch branch);
        IBranch GetNext();
    }
}
