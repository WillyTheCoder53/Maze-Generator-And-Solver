namespace MazeOOP
{
    public class Maze : Grid
    {
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }

        public int AnimationCooldownMS { get; set; }

        public bool GenerateWithAnimation { get; set; } = true; // option to solve the maze step by step
        public bool SolveWithAnimation { get; set; } = true; // option to generate the maze step by step

        public Maze(int height, int width) : base(height, width)
        {

            // Assign start and end locations (top left and bottom right)

            StartX = 1;
            StartY = 1;
            grid[StartY, StartX].IsStart = true;

            EndX = width - 3;
            EndY = height - 2;
            grid[EndY, EndX].IsEnd = true;
        }

        public void PrintMaze()
        {
            PrintGrid();
        }

        public void ResetVisited()
        {
            foreach (var cell in grid) cell.Visited = false;
        }

        public int CountPaths()
        {
            return CountPathsGrid();
        }
    }
}
