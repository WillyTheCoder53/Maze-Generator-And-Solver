namespace MazeOOP
{
    public class Grid : Square
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Square[,] grid; // Uses a 2d array of Squares to represent the grid

        private int previousy = 0;
        private int previousx = 0;

        public Grid(int height, int width)
        {
            Height = height;
            Width = width;
            grid = new Square[Height, Width];
            ConstructGrid();
        }

        private void ConstructGrid()
        {
            // Initilize each square
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    grid[y, x] = new Square();
                }
            }

            // Set the odd coordinates to path so the array resembles a grid

            for (int y = 1; y < Height - 1; y += 2)
            {
                for (int x = 1; x < Width - 1; x += 2)
                {
                    grid[y, x].IsPath = true;
                    grid[y, x].IsWall = false;
                }
            }
        }

        public void PrintGrid()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var cell = grid[y, x];

                    if (cell.IsDeadEnd) Console.BackgroundColor = ConsoleColor.Green;
                    else if (cell.IsWall) Console.BackgroundColor = ConsoleColor.Black;
                    else if (cell.IsSolvePath2 || cell.IsStart || cell.IsEnd) Console.BackgroundColor = ConsoleColor.Blue;
                    else if (cell.IsSolvePath3) Console.BackgroundColor = ConsoleColor.DarkBlue;
                    else if (cell.IsSolvePath) Console.BackgroundColor = ConsoleColor.DarkRed;
                    else Console.BackgroundColor = ConsoleColor.White;

                    Console.Write("  ");
                    Console.ResetColor();
                }
                if (y < Height - 1) Console.WriteLine(); // Don't write a new line for the last row
            }
        }

        public void DrawCell(int y, int x) // This is so a single cell can be redrawn without redrawing maze
        {
            var cell = grid[y, x];

            Console.SetCursorPosition(x * 2, y); // Each square is two wide "  "

            if (cell.IsDeadEnd) Console.BackgroundColor = ConsoleColor.Green;
            else if (cell.IsWall) Console.BackgroundColor = ConsoleColor.Black;
            else if (cell.IsSolvePath2 || cell.IsStart || cell.IsEnd) Console.BackgroundColor = ConsoleColor.Blue;
            else if (cell.IsSolvePath3) Console.BackgroundColor = ConsoleColor.DarkBlue;
            else if (cell.IsSolvePath) Console.BackgroundColor = ConsoleColor.DarkRed;
            else Console.BackgroundColor = ConsoleColor.White;

            Console.Write("  ");
            Console.ResetColor();
        }

        protected int CountPathsGrid()
        {
            int count = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (grid[y, x].IsPath) count++;
                }
            }
            return count;
        }

        public void SetCurrentCell(int y, int x, bool withAnimation)
        {
            if (previousx != 0 && previousy != 0)
            {
                grid[previousy, previousx].IsSolvePath = false;

                if (withAnimation) DrawCell(previousy, previousx);
            }

            grid[y, x].IsSolvePath = true;

            if (withAnimation) DrawCell(y, x);

            previousx = x;
            previousy = y;
        }

        public void RemoveSolvePath()
        {
            grid[previousy, previousx].IsSolvePath = false;
        }
    }
}
