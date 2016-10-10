using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    /// <summary>
    /// A node is a single state in a problem's state space with its own
    ///     heuristic and operations leading to next states
    /// </summary>
    interface INode
    {
        // A human friendly name for this state
        string getName();

        // Possible children states of this state
        // Mapping of children states along with the associated cost to move to said state
        Dictionary<INode, int> getOperations();

        // Heuristic value of the state
        int getHeuristic();

        // Compare to another node to check for equality
        bool getEquals(INode other);
    }
}
