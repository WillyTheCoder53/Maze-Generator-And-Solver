namespace MazeOOP
{
    public abstract class MazeGenerator
    {
        public Maze Maze { get; private set; }

        protected int Height;
        protected int Width;

        protected Square[,] Grid;

        protected int UnvisitedPaths;

        protected int CurrentX;
        protected int CurrentY;

        public abstract void GenerateMaze();

        public MazeGenerator(Maze maze)
        {
            Maze = maze;
            Height = maze.Height;
            Width = maze.Width;
            Grid = maze.grid;
            UnvisitedPaths = maze.CountPaths();
            CurrentX = 0;
            CurrentY = 0;
        }

        // Useful functions used by many of the generating algorithms

        protected virtual void connectTheCell(int startX, int startY, int endX, int endY)
        {
            int targetX;
            int targetY;

            if (startX == endX) // If horizontal is the same then it moved vertically
            {
                targetY = (startY + endY) / 2; // Get midpoint
                targetX = startX;
            }
            else // Must have moved horizontally
            {
                targetY = startY;
                targetX = (startX + endX) / 2; // Get midpoint
            }

            Grid[targetY, targetX].IsPath = true;
            Grid[targetY, targetX].IsWall = false;

            Maze.DrawCell(targetY, targetX);


        }
        protected virtual List<(int y, int x)> GetAdjacentCells(int startingY, int startingX)
        {
            List<(int y, int x)> adjacentCells = new List<(int, int)>();

            // Directions: Left -> Right -> Up -> Down in terms of y and x
            int[] changeInY = { 0, 0, 2, -2 };
            int[] changeInX = { -2, 2, 0, 0 };

            for (int i = 0; i < 4; i++) // Try all the possible directions
            {
                int newY = startingY + changeInY[i];
                int newX = startingX + changeInX[i];

                // Check if within grid
                if (newY >= 0 && newX >= 0)
                {
                    if (newY < Height - 1 && newX < Width - 1)
                    {
                        //if (Visited[newY, newX] == false) 
                        adjacentCells.Add((newY, newX));
                    }
                }
            }
            return adjacentCells;
        }

        protected virtual List<(int y, int x)> GetUnvisitedAdjacentCells(int startingY, int startingX)
        {
            List<(int y, int x)> adjacentCells = GetAdjacentCells(startingY, startingX);
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
        protected virtual List<(int y, int x)> GetVisitedAdjacentCells(int startingY, int startingX)
        {
            List<(int y, int x)> adjacentCells = GetAdjacentCells(startingY, startingX);
            List<(int y, int x)> visited = new List<(int, int)>();


            foreach (var cell in adjacentCells)
            {
                if (Grid[cell.y, cell.x].Visited)
                {
                    visited.Add((cell.y, cell.x));
                }
            }
            return visited;
        }


        protected (int y, int x) RandomPathCoordinates()
        {
            Random random = new Random();

            int x = random.Next(1, Width / 2) * 2 + 1;
            int y = random.Next(1, Height / 2) * 2 + 1;

            return (y, x);
        }
    }
}
