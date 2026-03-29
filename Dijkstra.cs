using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeOOP
{
    public class Dijkstra : MazeSolver
    {
        public Dijkstra(Maze maze) : base(maze) { }

        public override void SolveMaze()
        {
            PriorityQueue<(int y, int x), int> priorityQueue = new();
            List<(int y, int x)> solution = [];
            Dictionary<(int y, int x), (int y, int x)> parents = [];

            // All the cells distances are already set to infinity in Square.cs

            // 1. Set the start cell to a distance 0

            Grid[StartY, StartX].Distance = 0;

            // 2. Add the start cell to the priority queue with a priority of 0
            priorityQueue.Enqueue((StartY, StartX), 0);

            // 3. While the priority queue is not empty:

                // a. Remove and select the cell with the smallest distance
                // b. If that cell is the end stop the algorithm early
                // c. For every adjacent neighbour:
                        
                    // 

            while (priorityQueue.Count > 0)
            {
                var cell = priorityQueue.Dequeue();
                int currentDistance = getDistance(cell);

                if (currentDistance > getDistance(cell)) continue;

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
                        priorityQueue.Enqueue(path, newDistanceToPath);
                        Grid[path.y, path.x].Distance = newDistanceToPath;
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
        }
    }
}
