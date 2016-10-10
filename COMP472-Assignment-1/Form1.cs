using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP472_Assignment_1
{
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
        IFrontier frontier;
        List<IBranch> visited = new List<IBranch>();
        List<INode> goals = new List<INode>();
        bool DFSfix = false;

        public Form1()
        {
            InitializeComponent();
            cmbHType.DataSource = Enum.GetValues(typeof(EightPuzzleHeuristics));
            cmbProblemType.DataSource = Enum.GetValues(typeof(ProblemTypes));
            cmbSearchType.DataSource = Enum.GetValues(typeof(SearchType));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            for(int i =0; i < 9; i++)
            {
                Console.WriteLine("Index: " + i + " Point: " + EightPuzzleNode.indexToCoordinate(i).ToString());
            }*/

            

            return;

            loadBFS();
            loadSimpleData();
            performSearch();

            loadDFS();
            loadSimpleData();
            performSearch();

            loadGreedy();
            loadSimpleData();
            performSearch();

            loadAStar();
            loadSimpleData();
            performSearch();

            loadAStar();
            EightPuzzleNode.selectedHeuristic = EightPuzzleHeuristics.manhattan;
            loadEightPuzzleData();
            performSearch();

            loadAStar();
            EightPuzzleNode.selectedHeuristic = EightPuzzleHeuristics.row_swap;
            loadEightPuzzleData();
            performSearch();

            loadAStar();
            EightPuzzleNode.selectedHeuristic = EightPuzzleHeuristics.misplaced;
            loadEightPuzzleData();
            performSearch();

            loadAStar();
            EightPuzzleNode.selectedHeuristic = EightPuzzleHeuristics.row_col_count;
            loadEightPuzzleData();
            performSearch();

            //performASearch();

            //performBFS();
        }

        private void loadSearch()
        {
            //txtOut.Text = "";

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

            DFSfix = (SearchType)cmbSearchType.SelectedValue == SearchType.DFS;

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

        private void loadEightPuzzleData()
        {
            visited.Clear();
            goals.Clear();

            EightPuzzleNode goal = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.eight, Tiles.empty, Tiles.four, Tiles.seven, Tiles.six, Tiles.five });
            
            EightPuzzleNode.Goal = goal.getBoard();
            goals.Add(goal);

            //EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.eight, Tiles.six, Tiles.four, Tiles.seven, Tiles.five, Tiles.empty });
            //EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.eight, Tiles.empty, Tiles.four, Tiles.seven, Tiles.six, Tiles.five });

            //EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.one, Tiles.two, Tiles.three, Tiles.four, Tiles.five, Tiles.six, Tiles.eight, Tiles.seven, Tiles.empty });
            EightPuzzleNode start = new EightPuzzleNode(new List<Tiles> { Tiles.four, Tiles.six, Tiles.seven, Tiles.one, Tiles.empty, Tiles.two, Tiles.three, Tiles.five, Tiles.eight });
            
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
            var watch = System.Diagnostics.Stopwatch.StartNew();

            IBranch result = null;
            int count = 0;

            while (result == null)
            {
                count++;

                IBranch current = frontier.GetNext();
                visited.Add(current);

                //Console.WriteLine("    Checking path: " + current.printPath());
                if (count % 100 == 0)
                {
                    Console.WriteLine("    Checked " + count + " paths");
                }

                foreach (var op in current.getLeaf().getOperations())
                {
                    if (visited.Any(x => x.getLeaf().getEquals(op.Key)))
                    {
                        continue;
                    }

                    if (goals.Any(x => x.getEquals(op.Key)))
                    {
                        result = new SimpleBranch(op.Key, current);
                        break;
                    }

                    frontier.Add(new SimpleBranch(op.Key, current));
                }
            }


            watch.Stop();

            if (DFSfix)
            {
                // A quite bug fix to prevent the stack overflow error encountered here when performing DFS
                // The normal code analyzes the result recursively (causing an overflow in DFS's case), here it is done iteratively
                string path = "";
                int cost = 0;
                var last = result;
                while (last != null)
                {
                    path = last.getLeaf().getName() + " > " + path;
                    cost++;
                    last = last.getParent();
                }

                printMessage(string.Format("Found: {0}{1}    cost: {2}{3}    count: {4}{1}    After {5} seconds", path, System.Environment.NewLine, cost, System.Environment.NewLine, count, watch.Elapsed.TotalSeconds));
            }
            else
            {
                printMessage(string.Format("Found: {0}{1}    cost: {2}{3}    count: {4}{1}    After {5} seconds", result.printPath(), System.Environment.NewLine, result.getCost(), System.Environment.NewLine, count, watch.Elapsed.TotalSeconds));
            }
            //

            //printMessage("Found: " + result.printPath() + " cost: " + result.getCost());
            //printMessage("    count: " + count);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadSearch();
        }

        private void printMessage(string message)
        {
            txtOut.Text += message + System.Environment.NewLine + System.Environment.NewLine;
            Console.WriteLine(message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtOut.Text = "";
        }

        /*
        private void performBFS()
        {
            IBranch result = null;

            while (result == null)
            {
                IBranch current = frontier.Last();
                frontier.Remove(current);
                visited.Add(current);

                // Might have to move this after check
                if (goals.Any(x => x.getEquals(current.getLeaf())))
                {
                    result = current;
                    break;
                }

                foreach(var op in current.getLeaf().getOperations())
                {
                    if(visited.Any(x => x.getLeaf().getEquals(op.Key)))
                    {
                        continue;
                    }

                    if (goals.Any(x => x.getEquals(op.Key)))
                    {
                        result = new SimpleBranch(op.Key, current);
                        break;
                    }

                    frontier.Add(new SimpleBranch(op.Key, current));
                }
            }

            Console.WriteLine("Found: " + result.printPath() + " cost: " + result.getCost());
            Console.ReadLine();
        }

        private void performASearch()
        {
            IBranch result;

            while (true)
            {

                IBranch current = frontier.MinBy(x => x.getCost() + x.getLeaf().getHeuristic());

                Console.WriteLine("Exploring " + current.getLeaf().getName());

                if (goals.Any(x => x.getEquals(current.getLeaf())))
                {
                    result = current;
                    break;
                }

                frontier.Remove(current);
                visited.Add(current);

                foreach (var op in current.getLeaf().getOperations())
                {
                    // Check if we have already visited a the node
                    if (visited.Count(x => x.getLeaf().getEquals(op.Key)) > 0)
                    {
                        continue;
                    }

                    // If we found a path to a node that is already in the frontier, check to see if it is better and replace it
                    var frontierContenders = frontier.Where(x => x.getLeaf().getEquals(op.Key));
                    if (frontierContenders.Count() > 0)
                    {
                        IBranch contender = frontierContenders.MinBy(x => x.getCost() + x.getLeaf().getHeuristic());
                        if (contender.getCost() + contender.getLeaf().getHeuristic() > op.Key.getHeuristic() + op.Value + current.getCost())
                        {
                            frontier.Remove(contender);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // Add the valid child nodes to the frontier
                    frontier.Add(new SimpleBranch(op.Key, current));
                }
            }


            Console.WriteLine("Found: " + result.printPath() + " cost: " + result.getCost());
            Console.ReadLine();
        }*/
    }
}
