namespace MazeOOP
{
    public class Square // The maze is built out of these squares
    {
        public bool IsWall = true;
        public bool IsPath = false;
        public bool IsDeadEnd = false; // Used for Dead-end filling
        public bool IsStart = false;
        public bool IsEnd = false;
        public bool IsSolvePath = false; // Used to display how the algorithm is solving the maze
        public bool IsSolvePath2 = false; // Another colour option 
        public bool IsSolvePath3 = false; // Another colour option
        public bool Visited = false;
        public int Distance = 999999999; // Used for dijkstra's algorithm (999999999 represents infinity)

        public Square() { }
    }
}

