namespace MazeOOP
{
    public class WallFollower : MazeSolver
    {
        public WallFollower(Maze maze) : base(maze) { }

        public override void SolveMaze()
        {
            string orientation = "down"; // original orientation
            CurrentX = StartX;
            CurrentY = StartY;

            while (!Grid[CurrentY, CurrentX].IsEnd)
            {
                string left = TurnLeft(orientation);
                string right = TurnRight(orientation);
                string back = TurnBack(orientation);

                // Try move left then forward then right then back

                if (CanMove(left))
                {
                    Move(left);
                    orientation = left;
                }
                else if (CanMove(orientation)) // This is forward
                {
                    Move(orientation);
                }
                else if (CanMove(right))
                {
                    Move(right);
                    orientation = right;
                }
                else if (CanMove(back))
                {
                    Move(back);
                    orientation = back;
                }

                
                if (Maze.SolveWithAnimation)
                {
                    Grid[CurrentY, CurrentX].IsSolvePath = true;
                    Maze.DrawCell(CurrentY, CurrentX);
                    Thread.Sleep(Maze.AnimationCooldownMS);
                    Grid[CurrentY, CurrentX].IsSolvePath = false;
                    Maze.DrawCell(CurrentY, CurrentX);
                }
            }

            if (Maze.SolveWithAnimation) // Marks the end cell
            {
                Grid[CurrentY, CurrentX].IsSolvePath = true;
                Maze.DrawCell(CurrentY, CurrentX);
            }
        }

        private bool CanMove(string direction)
        {
            int x = CurrentX, y = CurrentY;

            switch (direction)
            {
                case "up":
                    y -= 1; break;

                case "down":
                    y += 1; break;

                case "left":
                    x -= 1; break;

                case "right":
                    x += 1; break;
            }
            return Grid[y, x].IsPath;
        }

        private void Move(string direction)
        {
            switch (direction)
            {
                case "up":
                    CurrentY -= 1; break;

                case "down":
                    CurrentY += 1; break;

                case "left":
                    CurrentX -= 1; break;

                case "right":
                    CurrentX += 1; break;
            }
        }

        private string TurnLeft(string direction)
        {
            switch (direction)
            {
                case "up":
                    return "left";

                case "down":
                    return "right";

                case "left":
                    return "down";

                case "right":
                    return "up";
            }
            return "error";
        }

        private string TurnRight(string direction)
        {
            switch (direction)
            {
                case "up":
                    return "right";

                case "down":
                    return "left";

                case "left":
                    return "up";

                case "right":
                    return "down";
            }
            return "error";
        }

        private string TurnBack(string direction)
        {
            switch (direction)
            {
                case "up":
                    return "down";

                case "down":
                    return "up";

                case "left":
                    return "right";

                case "right":
                    return "left";
            }
            return "error";
        }
    }
}
