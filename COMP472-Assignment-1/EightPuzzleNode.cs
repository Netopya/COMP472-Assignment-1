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
        min_misplaced_manhattan,
        row_swap,
        row_col_count
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
        
        private int getEulerDistance()
        {
            int h = 0;
            int index = 0;
            foreach (var tile in board)
            {
                var goalCoordinate = indexToCoordinate(Goal.IndexOf(tile));
                var currentCoordiante = indexToCoordinate(index);

                h += (int)Math.Floor(Math.Sqrt(Math.Pow(goalCoordinate.X - currentCoordiante.X, 2) + Math.Pow(goalCoordinate.Y - currentCoordiante.Y, 2)));
                index++;
            }

            return h;
        }

        // My implementation of a heuristic that is better than the manhattan distance
        // Based on the idea presented here, https://courses.cs.washington.edu/courses/csep573/11wi/lectures/03-hsearch.pdf
        // My idea is loosely based off of the linear conflicts heuristic, with a significant shortcut that is only applicable to the 3x3 senario.
        // For each row, get all the tiles that are in the correct row but in the incorrect position relative to the goal.
        // If we can find two tiles that need to be moved in opposite directions, add 2 to the heuristic (represents the additional vertical moves).
        private int getRowSwaps()
        {
            int h = 0;

            for (int i = 0; i < 9; i=i+3)
            {
                var current = board.GetRange(i, 3);
                var goal = Goal.GetRange(i, 3);

                var correctRow = current.Union(goal);

                List<int> wrongPosition = new List<int>();

                foreach(var tile in correctRow)
                {
                    int currentIndex = board.IndexOf(tile);
                    int goalIndex = Goal.IndexOf(tile);

                    if(currentIndex != goalIndex)
                    {
                        wrongPosition.Add(currentIndex - goalIndex);
                    }
                }

                if(wrongPosition.Any(x => x > 0) && wrongPosition.Any(x => x < 0))
                {
                    h += 2;
                }
            }

            return h;
        }

        private int getRowColCount()
        {
            int h = 0;

            for (int i = 0; i < 9; i = i + 3)
            {
                var current = board.GetRange(i, 3);
                var goal = Goal.GetRange(i, 3);

                var correctRow = current.Union(goal);

                h += 3 - correctRow.Count();
            }

            for(int i = 0; i < 3; i++)
            {
                var current = new List<Tiles> { board[i], board[i + 3], board[i + 6] };
                var goal = new List<Tiles> { Goal[i], Goal[i + 3], Goal[i + 6] };

                var correctCol = current.Union(goal);

                h += 3 - correctCol.Count();
            }

            return h;
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
                case EightPuzzleHeuristics.row_swap:
                    return getManhattanH() + getRowSwaps();
                case EightPuzzleHeuristics.row_col_count:
                    return getMisplacedH() + getRowColCount();
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
