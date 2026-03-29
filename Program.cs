using MazeOOP;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

int width; // Limit 56 - Even width only
int height; // Limit 35 - Odd height only

Stopwatch stopwatch = new Stopwatch();

while (true)
{
    Console.WriteLine("Would you like a small medium or large maze? (s/m/l)\n");
    string userChoice = Console.ReadLine().ToLower();

    if (userChoice == "small" || userChoice == "s")
    {
        height = 17;
        width = 22;
        break;
    }
    else if (userChoice == "medium" || userChoice == "m")
    {
        height = 25;
        width = 40;
        break;
    }
    else if (userChoice == "large" || userChoice == "l")
    {
        // Large by default so no need to adjust height or width
        height = 35;
        width = 56;
        break;
    }
    Console.Clear();
}

Maze maze = new Maze(height, width);

MazeGenerator generator;
MazeSolver solver;

while (true)
{
    Console.WriteLine();
    Console.WriteLine(@"What maze generator would you like to use?
1. Aldous Broder
2. Recursive Backtracking
3. Prim's algorithm
4. Eller's algorithm");
    Console.WriteLine();

    string userChoice = Console.ReadLine().ToLower();
    if (userChoice == "1")
    {
        generator = new AldousBroder(maze);
        break;
    }
    else if (userChoice == "2")
    {
        generator = new RecursiveBacktracking(maze); 
        break;
    }
    else if (userChoice == "3")
    {
        generator = new Prims(maze);
        break;
    }
    else if (userChoice == "4")
    {
        generator = new Ellers(maze);
        break;
    }
}

while (true)
{
    Console.WriteLine("\nWould you like to see the maze being generated? (y/n)\n");
    string userChoice = Console.ReadLine().ToLower();

    if (userChoice == "no" || userChoice == "n")
    {
        maze.GenerateWithAnimation = false;
        break;
    }
    else if (userChoice == "yes" || userChoice == "y")
    {
        maze.GenerateWithAnimation = true;
        break;
    }
}

if (maze.GenerateWithAnimation) // Only give the option for animation speed if they want an animation
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(@"What speed do you want the animations to be? (Type the number)

1. Fast
2. Medium
3. Slow");
        Console.WriteLine();

        string userChoice = Console.ReadLine();

        if (userChoice == "1")
        {
            maze.AnimationCooldownMS = 50;
            break;
        }
        else if (userChoice == "2")
        {
            maze.AnimationCooldownMS = 100;
            break;
        }
        else if (userChoice == "3")
        {
            maze.AnimationCooldownMS = 200;
            break;
        }
    }
}


stopwatch.Start();
Console.CursorVisible = false;
generator.GenerateMaze();
stopwatch.Stop();
Console.CursorVisible = true;

maze.PrintMaze();

Console.WriteLine($"\nThe maze was generated in {stopwatch.Elapsed.TotalSeconds} seconds (including the time taken for the animation)");

while (true)
{
    Console.WriteLine();
    Console.WriteLine(@"What maze solver would you like to use?
1. Dead-end filling
2. Depth first search
3. Breadth first search
4. Wall follower (stick to the right wall) - Have animation enabled for this algorithm
5. Dijkstra's algorithm
6. A* (An improved version of Dijsktra's algorithm)");
    Console.WriteLine();

    string userChoice = Console.ReadLine().ToLower();

    if (userChoice == "1")
    {
        solver = new DeadEndFilling(maze);
        break;
    }
    else if (userChoice == "2")
    {
        solver = new DepthFirstSearch(maze);
        break;
    }
    else if (userChoice == "3")
    {
        solver = new BreadthFirstSearch(maze);
        break;
    }
    else if (userChoice == "4")
    {
        solver = new WallFollower(maze);
        break;
    }
    else if (userChoice == "5")
    {
        solver = new Dijkstra(maze);
        break;
    }
    else if (userChoice == "6")
    {
        solver = new aStar(maze);
        break;
    }
}

while (true)
{
    Console.WriteLine("\nWould you like to see the maze being solved? (y/n)\n");
    string userChoice = Console.ReadLine().ToLower();

    if (userChoice == "no" || userChoice == "n")
    {
        maze.SolveWithAnimation = false;
        break;
    }
    else if (userChoice == "yes" || userChoice == "y")
    {
        maze.SolveWithAnimation = true;
        break;
    }
}

if (maze.SolveWithAnimation) // Only give the option for animation speed if they want an animation
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(@"What speed do you want the animations to be? (Type the number)

1. Fast
2. Medium
3. Slow");
        Console.WriteLine();

        string userChoice = Console.ReadLine();

        if (userChoice == "1")
        {
            maze.AnimationCooldownMS = 50;
            break;
        }
        else if (userChoice == "2")
        {
            maze.AnimationCooldownMS = 100;
            break;
        }
        else if (userChoice == "3")
        {
            maze.AnimationCooldownMS = 200;
            break;
        }
    }
}

maze.ResetVisited(); // This is so generation and solving Cell.Visited do not interfer with each other
maze.PrintMaze();

Console.CursorVisible = false;

// Time how long it takes to solve the maze
stopwatch.Restart();
solver.SolveMaze();
stopwatch.Stop();

maze.PrintMaze();

Console.CursorVisible = true;

Console.WriteLine($"\nThe maze was solved in {stopwatch.Elapsed.TotalSeconds} seconds (including the time taken for the animation");