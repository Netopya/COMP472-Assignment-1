using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP472_Assignment_1
{
    enum Tiles
    {
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        empty
    }

    enum EightPuzzleHeuristics
    {
        manhattan,
        misplaced,
        min_misplaced_manhattan
    }

    class EightPuzzleNode : INode
    {
        enum Operations
        {
            up,
            down,
            left,
            right
        }

        static readonly Dictionary<int, List<Operations>> validActions = new Dictionary<int, List<Operations>> {
            {0,new List<Operations> {Operations.right, Operations.down } },
            {1,new List<Operations> {Operations.right, Operations.down, Operations.left } },
            {2,new List<Operations> {Operations.left, Operations.down } },
            {3,new List<Operations> {Operations.right, Operations.down, Operations.up } },
            {4,new List<Operations> {Operations.right, Operations.down, Operations.up, Operations.left } },
            {5,new List<Operations> {Operations.left, Operations.down, Operations.up } },
            {6,new List<Operations> {Operations.right, Operations.up } },
            {7,new List<Operations> {Operations.right, Operations.up, Operations.left } },
            {8,new List<Operations> {Operations.left, Operations.up } },
        };

        static readonly Dictionary<Tiles, string> tileNames = new Dictionary<Tiles, string>
        {
            {Tiles.one, "1" },
            {Tiles.two, "2" },
            {Tiles.three, "3" },
            {Tiles.four, "4" },
            {Tiles.five, "5" },
            {Tiles.six, "6" },
            {Tiles.seven, "7" },
            {Tiles.eight, "8" },
            {Tiles.empty, "B" }
        };

        private List<Tiles> board = new List<Tiles>(9);
        private Dictionary<INode, int> operations;

        public static List<Tiles> Goal { get; set; }
        public static EightPuzzleHeuristics selectedHeuristic { get; set; }

        public EightPuzzleNode(List<Tiles> board)
        {
            this.board = board;
        }

        public List<Tiles> getBoard()
        {
            return board;
        }

        private int getMisplacedH()
        {
            return board.Zip(Goal, (f, s) => f == s).Count(x => !x);
        }

        private int getManhattanH()
        {
            int h = 0;
            int index = 0;
            foreach(var tile in board)
            {
                var goalCoordinate = indexToCoordinate(Goal.IndexOf(tile));
                var currentCoordiante = indexToCoordinate(index);

                h += Math.Abs(goalCoordinate.X - currentCoordiante.X) + Math.Abs(goalCoordinate.Y - currentCoordiante.Y);
                index++;
            }

            return h;
        }

        private Point indexToCoordinate(int index)
        {
            return new Point(index % 3, index / 3);
        }

        #region INode methods
        public bool getEquals(INode other)
        {
            EightPuzzleNode otherNode = (EightPuzzleNode)other;
            return board.SequenceEqual(otherNode.getBoard());
        }

        public int getHeuristic()
        {
            switch(selectedHeuristic)
            {
                case EightPuzzleHeuristics.misplaced:
                    return getMisplacedH();
                case EightPuzzleHeuristics.manhattan:
                    return getManhattanH();
                case EightPuzzleHeuristics.min_misplaced_manhattan:
                    return Math.Min(getMisplacedH(), getManhattanH());
                default:
                    throw new Exception("Could not load heuristic");
            }
        }

        public string getName()
        {
            return string.Format("({0})",string.Join(" ",board.Select(x => tileNames[x])));
        }

        public Dictionary<INode, int> getOperations()
        {
            if(operations != null)
            {
                return operations;
            }

            operations = new Dictionary<INode, int>();

            int pivot = board.IndexOf(Tiles.empty);

            foreach (var op in validActions[pivot])
            {
                int tileToMove = 0;
                switch (op)
                {
                    case Operations.down:
                        tileToMove = pivot + 3;
                        break;
                    case Operations.left:
                        tileToMove = pivot - 1;
                        break;
                    case Operations.right:
                        tileToMove = pivot + 1;
                        break;
                    case Operations.up:
                        tileToMove = pivot - 3;
                        break;
                    default:
                        throw new Exception("Could not move tile!");
                }

                List<Tiles> newBoard = new List<Tiles>(board);

                newBoard[pivot] = board[tileToMove];
                newBoard[tileToMove] = Tiles.empty;

                operations.Add(new EightPuzzleNode(newBoard), 1);
            }

            return operations;
        }
        #endregion
    }
}
