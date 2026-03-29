namespace MazeOOP
{
    public class BreadthFirstSearch : MazeSolver
    {
        public BreadthFirstSearch(Maze maze) : base(maze) { }

        public override void SolveMaze()
        {
            Random random = new Random();

            Queue<(int y, int x)> queue = new();
            List<(int y, int x)> solution = new();

            Dictionary<(int y, int x), (int y, int x)> parents = new Dictionary<(int, int), (int, int)>();

            // 1. Add the starting cell to the queue

            queue.Enqueue((Maze.StartY, Maze.StartX));

            Grid[Maze.StartY, Maze.StartX].Visited = true;

            // While the queue is not empty (If the end is reached it stops early)

            while (queue.Count > 0)
            {
                var currentCoordinates = queue.Dequeue();
                CurrentY = currentCoordinates.y;
                CurrentX = currentCoordinates.x;

                if (Maze.SolveWithAnimation) Grid[CurrentY, CurrentX].IsSolvePath3 = true;

                if (Grid[CurrentY, CurrentX].IsEnd) break;

                Maze.DrawCell(CurrentY, CurrentX);

                var unvisitedAdjacentPaths = GetUnvisitedAdjacentPaths(CurrentY, CurrentX);

                foreach (var path in unvisitedAdjacentPaths)
                {
                    Grid[path.y, path.x].Visited = true;
                    queue.Enqueue(path);
                    parents[path] = (CurrentY, CurrentX);
                    if (Maze.SolveWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);
                }
            }

            // Get the solution using the parents starting from the end
            var currentCell = (CurrentY, CurrentX);
            currentCell.CurrentX = Maze.EndX;
            currentCell.CurrentY = Maze.EndY;

            while (Grid[currentCell.CurrentY, currentCell.CurrentY].IsStart == false) // While not at the start
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
        }
    }
}
