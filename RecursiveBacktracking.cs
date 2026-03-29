namespace MazeOOP
{
    public class RecursiveBacktracking : MazeGenerator
    {
        public RecursiveBacktracking(Maze maze) : base(maze) { }

        public override void GenerateMaze()
        {
            if (Maze.GenerateWithAnimation)
            {
                Maze.PrintMaze();
                Thread.Sleep(Maze.AnimationCooldownMS);
            }

            Stack<(int y, int x)> previousPaths = new Stack<(int, int)>();
            Random random = new Random();
            var randomPath = RandomPathCoordinates();

            CurrentX = randomPath.x;
            CurrentY = randomPath.y;
            Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);

            previousPaths.Push((CurrentY, CurrentX));

            UnvisitedPaths--;
            Grid[CurrentY, CurrentX].Visited = true;

            while (UnvisitedPaths > 0)
            {
                int initialUnvisitedCount = UnvisitedPaths;
                List<(int y, int x)> adjacentCells = GetUnvisitedAdjacentCells(CurrentY, CurrentX);

                if (adjacentCells.Count == 0)
                {
                    if (previousPaths.Count == 0) break;

                    var previousCoordinates = previousPaths.Pop();
                    CurrentX = previousCoordinates.x;
                    CurrentY = previousCoordinates.y;
                    Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);
                }
                else
                {
                    (int adjacentY, int adjacentX) = adjacentCells[random.Next(adjacentCells.Count)];

                    Grid[adjacentY, adjacentX].Visited = true;
                    connectTheCell(CurrentX, CurrentY, adjacentX, adjacentY);
                    UnvisitedPaths--;
                    CurrentY = adjacentY;
                    CurrentX = adjacentX;
                    Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);
                    previousPaths.Push((CurrentY, CurrentX));
                }

                if (Maze.GenerateWithAnimation)
                {
                    if (initialUnvisitedCount != UnvisitedPaths) // If the amount of paths has changed
                    {
                        //Maze.PrintMaze();
                        Thread.Sleep(Maze.AnimationCooldownMS);
                    }
                }
            }
            Maze.RemoveSolvePath();
        }
        private bool alreadyVisitedAllAdjacent(List<(int y, int x)> adjacentCells)
        {
            foreach (var cell in adjacentCells)
            {
                if (Grid[cell.y, cell.x].Visited == false) return false;
            }
            return true;
        }
    }
}