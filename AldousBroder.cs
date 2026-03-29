namespace MazeOOP
{
    public class AldousBroder : MazeGenerator
    {
        private List<(int y, int x)> adjacentCells = [];

        public AldousBroder(Maze maze) : base(maze) { }

        public override void GenerateMaze()
        {
            if (Maze.GenerateWithAnimation)
            {
                Maze.PrintMaze();
                Thread.Sleep(Maze.AnimationCooldownMS);
            }

            Random random = new Random();

            // 1: Pick a random path from the maze (which is a grid of paths currently) to start at

            var randomPath = RandomPathCoordinates();

            CurrentX = randomPath.x;
            CurrentY = randomPath.y;

            Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);
            if (Maze.GenerateWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);

            UnvisitedPaths--;
            Grid[CurrentY, CurrentX].Visited = true;


            // 2: Get the adjacent cells and randomally connect to one of them. Repeat this until all cells have been visited

            while (UnvisitedPaths > 0)
            {
                adjacentCells = GetAdjacentCells(CurrentY, CurrentX);

                var randomAdjacentCell = adjacentCells[random.Next(adjacentCells.Count)];
                int adjacentY = randomAdjacentCell.y;
                int adjacentX = randomAdjacentCell.x;

                if (Grid[adjacentY, adjacentX].Visited == false)
                {
                    Grid[adjacentY, adjacentX].Visited = true;
                    UnvisitedPaths--;
                    connectTheCell(CurrentX, CurrentY, adjacentX, adjacentY);
                }

                CurrentX = adjacentX;
                CurrentY = adjacentY;

                Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);
                if (Maze.GenerateWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);
            }
            Maze.RemoveSolvePath();
        }
    }
}
