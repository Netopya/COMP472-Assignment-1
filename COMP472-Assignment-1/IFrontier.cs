using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// An interface to define the frontier collection of a search algorithm
    /// By implementing the underlying logic of the order in which items are
    ///     inserted and retrieved, different properties can be achieved
    /// </summary>
    interface IFrontier
    {
        void Add(IBranch branch);
        IBranch GetNext();
    }
}
