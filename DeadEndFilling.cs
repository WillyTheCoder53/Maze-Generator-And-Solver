namespace MazeOOP
{
    public class DeadEndFilling : MazeSolver
    {

        public DeadEndFilling(Maze maze) : base(maze) { }

        public override void SolveMaze()
        {
            int pathCellsBefore = 0;

            while (pathCellsBefore != Maze.CountPaths()) // Repeat until the path count does not change as that must mean the maze is solved
            {
                pathCellsBefore = Maze.CountPaths();
                // Step 1: Go through each cell in the maze and if it is a dead ends fill in (dead end = 3 adjacent walls)

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (Grid[y, x].IsPath && Grid[y, x].IsStart == false && Grid[y, x].IsEnd == false) // If this coordinate is part of the path 
                        {
                            CurrentX = x;
                            CurrentY = y;

                            if (CountAdjacentWalls(CurrentY, CurrentX) >= 3) // Due to errors in maze generation, it is possible to have 4 adjacent walls
                            {
                                // Set the cell to DeadEnd as it has 3 adjacent walls or remove it depending on animation settings

                                Grid[CurrentY, CurrentX].IsDeadEnd = true;
                                Grid[CurrentY, CurrentX].IsPath = false;
                                Grid[CurrentY, CurrentX].IsWall = true;

                                if (Maze.SolveWithAnimation)
                                {
                                    Maze.DrawCell(CurrentY, CurrentX);
                                    Thread.Sleep(Maze.AnimationCooldownMS);
                                }
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = Grid[y, x];
                    // Change the colour of the remaining paths so its clear they are the solution
                    if (cell.IsPath)
                    {
                        cell.IsSolvePath2 = true;
                        cell.IsPath = false;
                    }
                    if (cell.IsDeadEnd)
                    {
                        cell.IsDeadEnd = false;
                        cell.IsPath = true;
                        cell.IsWall = false;
                    }
                }
            }
            if (Maze.SolveWithAnimation) Thread.Sleep(1000);
        }
    }
}
