namespace MazeOOP
{
    public class Prims : MazeGenerator
    {
        public Prims(Maze maze) : base(maze) { }

        public override void GenerateMaze()
        {
            if (Maze.GenerateWithAnimation)
            {
                Maze.PrintMaze();
                Thread.Sleep(Maze.AnimationCooldownMS);
            }

            // 1. Visited a random cell

            Random random = new Random();
            List<(int y, int x)> frontier = new();
            (int y, int x) randomFrontier;

            (CurrentY, CurrentX) = RandomPathCoordinates();
            Grid[CurrentY, CurrentX].Visited = true;
            Maze.SetCurrentCell(CurrentY, CurrentX, Maze.GenerateWithAnimation);

            // Add the neighbours of the starting cell (refer to these as frontiers)
            frontier.AddRange(GetAdjacentCells(CurrentY, CurrentX));

            // 2. While the set > 0

            while (frontier.Count > 0)
            {
                // 3. Pick a random frontier cell

                int randomIndex = random.Next(frontier.Count);

                randomFrontier = frontier[randomIndex];
                frontier.RemoveAt(randomIndex);

                // 4. Get all visited neighbours of that frontier cell
                var visited = GetVisitedAdjacentCells(randomFrontier.y, randomFrontier.x);
                if (visited.Count == 0) continue;

                // 5. Pick a random visited neighbour and connect the frontier cell to it

                randomIndex = random.Next(visited.Count);

                (int y, int x) randomNeighbour = visited[randomIndex];
                //visited.RemoveAt(randomIndex);

                connectTheCell(randomFrontier.x, randomFrontier.y, randomNeighbour.x, randomNeighbour.y);

                Maze.SetCurrentCell(randomFrontier.y, randomFrontier.x, Maze.GenerateWithAnimation);

                Grid[randomFrontier.y, randomFrontier.x].Visited = true;

                // Add the other neighbours of the chosen random frontier that are not being checked this time round


                foreach (var neighbour in GetUnvisitedAdjacentCells(randomFrontier.y, randomFrontier.x))
                {
                    if (!frontier.Contains(neighbour))
                    {
                        frontier.Add(neighbour);
                    }
                }

                if (Maze.GenerateWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);

            }
            Maze.RemoveSolvePath();
        }
    }
}