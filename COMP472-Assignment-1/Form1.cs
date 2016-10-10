using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    Assignment #1 for COMP 472 Artificial Intelligence
    Michael Bilinsky 26992358
    By implementing a generic search algorithm, various searches are implented by extending the IFrontier and INode interfaces
    BFS, DFS, Greedy, and A* Star searches can be performed on a simple graph and 8-puzzle problems
    For the informed searches, Manhattan Distance, Misplaced Tiles, the Minimum of Manhattan and Misplaced, along with
        two heuristics of my own design (Row-Swap and RowColCount) can be used
    Execution times are recorded along with the cost and iteration count of the solution

    The A* Star search uses an OrderedBag datastructure from Wintellect's Power Collection for .NET library
    source: https://powercollections.codeplex.com/
*/

namespace COMP472_Assignment_1
{
    // Enums to aid in problem configuration

    enum ProblemTypes
    {
        Graph,
        Eight_puzzle
    }

    enum SearchType
    {
        BFS,
        DFS,
        Greedy,
        AStar
    }

    public partial class Form1 : Form
    {
        IFrontier frontier; // The collection of nodes to explore
        List<IBranch> visited = new List<IBranch>(); // The collection of visited nodes
        List<INode> goals = new List<INode>(); // The collection of goal states
        bool DFSfix = false; // A quick fix to avoid stack overflow exceptions during DFS and Greedy searches

        public Form1()
        {
            InitializeComponent();

            // Initialize the drop down menus
            cmbHType.DataSource = Enum.GetValues(typeof(EightPuzzleHeuristics));
            cmbProblemType.DataSource = Enum.GetValues(typeof(ProblemTypes));
            cmbSearchType.DataSource = Enum.GetValues(typeof(SearchType));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void loadSearch()
        {
            switch((SearchType)cmbSearchType.SelectedValue)
            {
                case SearchType.BFS:
                    loadBFS();
                    break;
                case SearchType.DFS:
                    loadDFS();
                    break;
                case SearchType.Greedy:
                    loadGreedy();
                    break;
                case SearchType.AStar:
                    loadAStar();
                    break;
            }

            DFSfix = (SearchType)cmbSearchType.SelectedValue == SearchType.DFS || (SearchType)cmbSearchType.SelectedValue == SearchType.Greedy;

            if ((ProblemTypes)cmbProblemType.SelectedValue == ProblemTypes.Graph)
            {
                loadSimpleData();
            }
            else
            {
                EightPuzzleNode.selectedHeuristic = (EightPuzzleHeuristics)cmbHType.SelectedValue;
                printMessage("Setting heuristic to " + cmbHType.SelectedValue);
                loadEightPuzzleData();
            }

            performSearch();
        }

        /// <summary>
        /// Load a simple graph problem as shown in the first tutorial
        /// </summary>
        private void loadSimpleData()
        {
            visited.Clear();
            goals.Clear();

            SimpleNode S = new SimpleNode(10, "S");
            SimpleNode A = new SimpleNode(5, "A");
            SimpleNode B = new SimpleNode(8, "B");
            SimpleNode C = new SimpleNode(3, "C");
            SimpleNode D = new SimpleNode(2, "D");
            SimpleNode E = new SimpleNode(4, "E");
            SimpleNode G1 = new SimpleNode(0, "G1");
            SimpleNode G2 = new SimpleNode(0, "G2");

            S.MakeLink(3, A);
            S.MakeLink(7, B);
            A.MakeLink(1, C);
            A.MakeLink(6, D);
            B.MakeLink(1, E);
            B.MakeLink(9, G2);
            C.MakeLink(2, S);
            C.MakeLink(4, D);
            D.MakeLink(6, G1);
            D.MakeLink(3, B);
            E.MakeLink(5, G2);
            G1.MakeLink(2, C);
            G2.MakeLink(8, B);

            frontier.Add(new SimpleBranch(S, null));
            goals.Add(G1);
            goals.Add(G2);
        }

        /// <summary>
        /// Load the 8-Puzzle
        /// </summary>
        private void loadEightPuzzleData()
        {
            visited.Clear();
            goals.Clear();

            EightPuzzleNode goal = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.eight, Tiles.empty, Tiles.four, Tiles.seven, Tiles.six, Tiles.five });
            
            EightPuzzleNode.Goal = goal.getBoard();
            goals.Add(goal);

            EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.four, Tiles.five, Tiles.six, Tiles.eight, Tiles.seven, Tiles.empty });
            //EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.four, Tiles.six, Tiles.seven, Tiles.one, Tiles.empty, Tiles.two, Tiles.three, Tiles.five, Tiles.eight });
            
            frontier.Add(new SimpleBranch(start, null));
        }

        private void loadBFS()
        {
            printMessage("Loading BFS lists");
            frontier = new BFSFrontierList();
        }

        private void loadDFS()
        {
            printMessage("Loading DFS list");
            frontier = new DFSFrontierList();
        }

        private void loadGreedy()
        {
            printMessage("Loading Greedy search");
            frontier = new GreedyFrontierList();
        }

        private void loadAStar()
        {
            printMessage("Loading A* Search search");
            frontier = new AStarFrontierList();
        }


        private void performSearch()
        {
            // Start the timer
            var watch = System.Diagnostics.Stopwatch.StartNew();

            IBranch result = null;
            int count = 0;

            // Iterate until a result is found
            while (result == null)
            {
                count++;

                // Get the next item to explore in the frontier and add it to visited branches
                IBranch current = frontier.GetNext();
                visited.Add(current);

                // At 100 iteration intervals, print that we are still counting to let the user know we are still working
                if (count % 100 == 0)
                {
                    Console.WriteLine("    Checked " + count + " paths");
                }

                // Check each operation available at the current node
                foreach (var op in current.getLeaf().getOperations())
                {
                    // Check to see if the new move has already been visited
                    if (visited.Any(x => x.getLeaf().getEquals(op.Key)))
                    {
                        continue;
                    }

                    // If we found a goal state, exit the loop
                    if (goals.Any(x => x.getEquals(op.Key)))
                    {
                        result = new SimpleBranch(op.Key, current);
                        break;
                    }

                    // Add the node to the frontier by creating a new leaf with the current node as the parent
                    frontier.Add(new SimpleBranch(op.Key, current));
                }
            }


            watch.Stop();

            /*
                Report the solution's path, cost, count, and execution time
            */


            if (DFSfix)
            {
                // A quite bug fix to prevent the stack overflow error encountered here when performing DFS
                // The normal code analyzes the result recursively (causing an overflow in DFS's case), here it is done iteratively
                string path = result.getLeaf().getName();
                int cost = 0;
                var last = result;
                while (last != null)
                {
                    cost++;
                    last = last.getParent();
                }

                printMessage(string.Format("Found: {0}{1}    cost: {2}{3}    count: {4}{1}    After {5} seconds", path, System.Environment.NewLine, cost, System.Environment.NewLine, count, watch.Elapsed.TotalSeconds));
            }
            else
            {
                printMessage(string.Format("Found: {0}{1}    cost: {2}{3}    count: {4}{1}    After {5} seconds", result.printPath(), System.Environment.NewLine, result.getCost(), System.Environment.NewLine, count, watch.Elapsed.TotalSeconds));
            }
        }

        // The "Run!" button
        private void button1_Click(object sender, EventArgs e)
        {
            loadSearch();
        }

        // Print a message to the GUI textbox and the console
        private void printMessage(string message)
        {
            txtOut.Text += message + System.Environment.NewLine + System.Environment.NewLine;
            Console.WriteLine(message);
        }

        // The clear output button
        private void button2_Click(object sender, EventArgs e)
        {
            txtOut.Text = "";
        }
    }
}
