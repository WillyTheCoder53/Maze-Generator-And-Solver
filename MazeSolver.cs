namespace MazeOOP
{
    public abstract class MazeSolver
    {
        public Maze Maze { get; set; }

        protected int Height;
        protected int Width;

        protected int CurrentX = 0;
        protected int CurrentY = 0;

        protected int StartX;
        protected int StartY;

        protected int UnvisitedPaths;

        protected Square[,] Grid;


        public MazeSolver(Maze maze)
        {
            Maze = maze;
            UnvisitedPaths = maze.CountPaths();
            Grid = maze.grid;
            Height = maze.Height;
            Width = maze.Width;
            StartX = maze.StartX;
            StartY = maze.StartY;
        }

        public abstract void SolveMaze();

        // Useful solving functions used by many of the algorithms:

        protected int CountAdjacentWalls(int startingY, int startingX)
        {
            int count = 0;

            int[] changeInY = { 0, 0, 1, -1 };
            int[] changeInX = { -1, 1, 0, 0 };


            for (int i = 0; i < 4; i++) // Try all the possible directions
            {
                int newY = startingY + changeInY[i];
                int newX = startingX + changeInX[i];

                // Check if within grid
                if (newY >= 0 && newX >= 0)
                {
                    if (newY < Height && newX < Width)
                    {
                        if (Grid[newY, newX].IsWall) count++;
                    }
                }
            }
            return count;
        }

        protected virtual List<(int y, int x)> GetUnvisitedAdjacentPaths(int startingY, int startingX)
        {
            List<(int y, int x)> adjacentCells = GetAdjacentPaths(startingY, startingX);
            List<(int y, int x)> unvisited = new List<(int, int)>();


            foreach (var cell in adjacentCells)
            {
                if (Grid[cell.y, cell.x].Visited == false)
                {
                    unvisited.Add((cell.y, cell.x));
                }
            }
            return unvisited;
        }

        protected virtual List<(int y, int x)> GetAdjacentPaths(int startingY, int startingX)
        {
            List<(int y, int x)> adjacentCells = new List<(int, int)>();

            // Directions: Left -> Right -> Up -> Down in terms of y and x
            int[] changeInY = { 0, 0, 1, -1 };
            int[] changeInX = { -1, 1, 0, 0 };

            for (int i = 0; i < 4; i++) // Try all the possible directions
            {
                int newY = startingY + changeInY[i];
                int newX = startingX + changeInX[i];

                // Check if within grid
                if (newY >= 0 && newX >= 0)
                {
                    if (newY < Height - 1 && newX < Width - 1 && Grid[newY, newX].IsPath)
                    {
                        //if (Visited[newY, newX] == false) 
                        adjacentCells.Add((newY, newX));
                    }
                }
            }
            return adjacentCells;
        }
    }
}
