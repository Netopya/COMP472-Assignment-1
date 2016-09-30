using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoreLinq;

namespace COMP472_Assignment_1
{
    public partial class Form1 : Form
    {
        List<IBranch> frontier = new List<IBranch>();
        List<IBranch> visited = new List<IBranch>();
        List<INode> goals = new List<INode>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadSimpleData();
            performASearch();
            loadSimpleData();
            performBFS();
        }

        private void loadSimpleData()
        {
            frontier.Clear();
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
        }
    }
}
