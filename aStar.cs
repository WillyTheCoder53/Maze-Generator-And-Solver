namespace MazeOOP
{
    public class aStar : MazeSolver
    {
        public aStar(Maze maze) : base(maze) { }

        private int heuristicWeight = 2; // This determines how valuable the distance from the end is vs the distance from the start

        public override void SolveMaze()
        {
            // -- NOTE --
            // Most of this was copy pasted from the Dijkstra's class
            // A heurestic was added, accounting for the distance from the end of the maze

            PriorityQueue<(int y, int x), int> priorityQueue = new();
            List<(int y, int x)> solution = [];
            Dictionary<(int y, int x), (int y, int x)> parents = [];

            // All the cells distances are already set to infinity in Square.cs

            // 1. Set the start cell to a distance 0

            Grid[StartY, StartX].Distance = 0;

            // 2. Add the start cell to the priority queue
            // Use a priority only based on the heurestic as the distance is 0

            int startPriority = heurestic((StartY, StartX));

            priorityQueue.Enqueue((StartY, StartX), startPriority);

            // 3. While the priority queue is not empty:

            // a. Remove and select the cell with the smallest distance
            // b. If that cell is the end stop the algorithm early
            // c. For every adjacent neighbour:

            // 

            while (priorityQueue.Count > 0)
            {
                var cell = priorityQueue.Dequeue();
                int currentDistance = getDistance(cell);

                if (Maze.SolveWithAnimation)
                {
                    Grid[cell.y, cell.x].IsSolvePath3 = true;
                    Maze.DrawCell(cell.y, cell.x);
                    Thread.Sleep(Maze.AnimationCooldownMS);
                }

                if (Grid[cell.y, cell.x].IsEnd) break;

                var paths = GetAdjacentPaths(cell.y, cell.x);

                foreach (var path in paths)
                {
                    int pathDistance = getDistance(path);
                    int newDistanceToPath = currentDistance + 1; // Only costs 1 more to move there from the current path

                    if (newDistanceToPath < pathDistance)
                    {
                        parents[path] = (cell.y, cell.x);
                        Grid[path.y, path.x].Distance = newDistanceToPath;

                        int priority = newDistanceToPath + heurestic(path);

                        priorityQueue.Enqueue(path, priority);
                    }
                }
            }

            // Get the solution to the maze using the same approach from breadth-first search:

            var currentCell = (CurrentY, CurrentX);
            currentCell.CurrentX = Maze.EndX;
            currentCell.CurrentY = Maze.EndY;

            while (currentCell != (Maze.StartY, Maze.StartX))
            {
                solution.Add(currentCell);
                currentCell = parents[currentCell];

                Grid[currentCell.CurrentY, currentCell.CurrentX].IsSolvePath = false;
                Grid[currentCell.CurrentY, currentCell.CurrentX].IsSolvePath2 = true;
                Grid[currentCell.CurrentY, currentCell.CurrentX].IsSolvePath3 = false;

                if (Maze.SolveWithAnimation)
                {
                    Maze.DrawCell(currentCell.CurrentY, currentCell.CurrentX);
                    Thread.Sleep(Maze.AnimationCooldownMS);
                }
            }

            if (Maze.SolveWithAnimation) Thread.Sleep(1000);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[y, x].IsSolvePath3 = false;
                }
            }

            int getDistance((int y, int x) cell)
            {
                return Grid[cell.y, cell.x].Distance;
            }

            int heurestic((int y, int x) cell)
            {
                // Uses the manhattan distance formula to estimate the distance from the goal (this is a lot more accurate for mazes)

                return heuristicWeight * (Math.Abs(cell.y - Maze.EndY) + Math.Abs(cell.x - Maze.EndX));
            }
        }
    }
}