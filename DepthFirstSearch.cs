namespace MazeOOP
{
    public class DepthFirstSearch : MazeSolver
    {
        public DepthFirstSearch(Maze maze) : base(maze) { }

        public override void SolveMaze()
        {
            Random random = new Random();

            Stack<(int y, int x)> stack = new Stack<(int, int)>();

            // 1. Add the starting cell to the stack

            stack.Push((Maze.StartY, Maze.StartX));

            // While the stack is not empty:

            while (stack.Count > 0)
            {
                var currentCoordinates = stack.Peek();
                CurrentY = currentCoordinates.y;
                CurrentX = currentCoordinates.x;

                Grid[CurrentY, CurrentX].Visited = true;

                if (Maze.SolveWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);

                if (Grid[CurrentY, CurrentX].IsEnd) break;

                // 2. Add a random unvisited neighbour to the stack if its not a dead-end

                var unvisitedNeighbourPath = GetUnvisitedNeighbouringPath(CurrentY, CurrentX);

                if (unvisitedNeighbourPath.x == -1) // IsDeadEnd
                {
                    // Remove from the stack
                    stack.Pop();

                    Grid[CurrentY, CurrentX].IsSolvePath2 = false;
                    Maze.DrawCell(CurrentY, CurrentX);
                }

                // Not dead end 
                else
                {
                    Grid[CurrentY, CurrentX].IsSolvePath2 = true;

                    Maze.DrawCell(CurrentY, CurrentX);
                    stack.Push(unvisitedNeighbourPath);
                }
            }
        }

        private (int y, int x) GetUnvisitedNeighbouringPath(int y, int x)
        {
            // Try down then right then up then left

            // Down
            if (Grid[y + 1, x].IsPath && Grid[y + 1, x].Visited == false) return (y + 1, x);

            // Right
            else if (Grid[y, x + 1].IsPath && Grid[y, x + 1].Visited == false) return (y, x + 1);

            // Up
            else if (Grid[y - 1, x].IsPath && Grid[y - 1, x].Visited == false) return (y - 1, x);

            // Left
            else if (Grid[y, x - 1].IsPath && Grid[y, x - 1].Visited == false) return (y, x - 1);

            else return (-1, -1);
            
        }
    }
}
