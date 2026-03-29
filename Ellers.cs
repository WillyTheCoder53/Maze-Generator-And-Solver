namespace MazeOOP
{
    public class Ellers : MazeGenerator
    {
        public Ellers(Maze maze) : base(maze) { }

        private Dictionary<int, List<(int y, int x)>> sets = [];

        int[,] cellSet;

        private int setCounter = 0;

        public override void GenerateMaze()
        {
            if (Maze.GenerateWithAnimation)
            {
                Maze.PrintMaze();
                Thread.Sleep(Maze.AnimationCooldownMS);
            }

            // Label all cells as no set:

            cellSet = new int[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0;  x < Width; x++)
                {
                    cellSet[y, x] = -1;
                }
            }

            Random random = new Random();

            for (CurrentY = Maze.StartY; CurrentY < Height - 2; CurrentY += 2)
            {
                for (CurrentX = Maze.StartX; CurrentX < Width - 2; CurrentX += 2) // + 2 so only checking original paths not walls or new paths
                {

                    // 1. Add all the cells on the current row without a set to their own unique set

                    if (BoolInASet(CurrentY, CurrentX) == false) CreateSet(CurrentY, CurrentX);
                }

                // 2. Consider pairs of cells: if they are in different sets coinflip (changed probability) to connect and merge them or do nothing (leaving a wall there)

                for (CurrentX = Maze.StartX; CurrentX < Width - 4; CurrentX += 2)
                {
                    int nextX = CurrentX + 2;

                    Grid[CurrentY, CurrentX].IsSolvePath = true;

                    if (Maze.GenerateWithAnimation)
                    {
                        Maze.DrawCell(CurrentY, CurrentX);
                        Thread.Sleep(Maze.AnimationCooldownMS);
                    }

                    if (BoolSameSet(CurrentY, CurrentX, CurrentY, nextX) == false)
                    {

                        if (random.Next(3) != 0) // changed probability to give more horizontal bias
                        {
                            connectTheCell(CurrentX, CurrentY, nextX, CurrentY);
                            MergeSets(CurrentY, nextX, CurrentY, CurrentX);
                        }
                    }

                    Grid[CurrentY, CurrentX].IsSolvePath = false;
                    Maze.DrawCell(CurrentY, CurrentX);
                }

                // 3. For each set, create a vertical connection downwards, adding the newly connected cell to the same set.
                // Make sure every set has at least one vertical connection!

                // Use sets.ToList() to effectively create a clone of sets so its not modifying what it is looping over


                foreach (var keyValuePair in sets)
                {

                    var groupOfCells = keyValuePair.Value;

                    var randomCell = groupOfCells[random.Next(groupOfCells.Count)];
                    CurrentX = randomCell.x; // Move to x coordinate of random cell

                    connectTheCell(CurrentX, CurrentY, CurrentX, CurrentY + 2);
                    AddToSet(CurrentY, CurrentX, CurrentY + 2, CurrentX);

                    Grid[CurrentY, CurrentX].IsSolvePath = true;
                    Maze.DrawCell(CurrentY, CurrentX);

                    if (Maze.GenerateWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);
                    Grid[CurrentY, CurrentX].IsSolvePath = false;
                    Maze.DrawCell(CurrentY, CurrentX);

                }

                // Clear the sets on this row as they are no longer needed saving memory and preventing bugs
                RemoveCellsFromSetWithCommonY(CurrentY);
                CleanupEmptySets();
            }


            // 4. For the last row: add all the cells without a set to their own unique set

            CurrentY = Maze.EndY; // This sets currentY to the last row

            for (CurrentX = Maze.StartX; CurrentX < Width - 4; CurrentX += 2)
            {
                if (BoolInASet(CurrentY, CurrentX) == false) CreateSet(CurrentY, CurrentX);
            }

            // 5. Then, for the last row: connect cells that are in different sets

            for (CurrentX = Maze.StartX; CurrentX < Width - 4; CurrentX += 2)
            {
                int nextX = CurrentX + 2;

                Grid[CurrentY, CurrentX].IsSolvePath = true;
                Maze.DrawCell(CurrentY, CurrentX);

                if (Maze.GenerateWithAnimation) Thread.Sleep(Maze.AnimationCooldownMS);

                if (BoolSameSet(CurrentY, CurrentX, CurrentY, nextX) == false)
                {
                    connectTheCell(CurrentX, CurrentY, nextX, CurrentY);
                    MergeSets(CurrentY, nextX, CurrentY, CurrentX);
                }

                Grid[CurrentY, CurrentX].IsSolvePath = false;
                Maze.DrawCell(CurrentY, CurrentX);
            }
        }

        private void CreateSet(int y, int x)
        {

            sets[setCounter] = new List<(int, int)> { (y, x) };
            cellSet[y, x] = setCounter;
            setCounter++;
        }

        private void RemoveCellsFromSetWithCommonY(int y)
        {

            foreach (var keyValuePairs in sets)
            {
                var cells = keyValuePairs.Value.ToList();

                for (int x = cells.Count - 1; x >= 0; x--) // Iterate backwards to try prevent errors
                {
                    if (cells[x].y == y)
                    {
                        sets[keyValuePairs.Key].Remove(cells[x]);

                        cellSet[cells[x].y, cells[x].x] = -1; // Mark that cell as no set
                    }
                }
            }
        }

        private void CleanupEmptySets()
        {
            foreach (var set in sets)
            {
                if (set.Value.Count == 0) // If set is empty
                {
                    var key = set.Key;
                    sets.Remove(key);
                }
            }
        }

        private void MergeSets(int fromY, int fromX, int toY, int toX)
        {
            int fromSet = GetSetID(fromY, fromX);
            int toSet = GetSetID(toY, toX);

            if (fromSet == -1 || toSet == -1) return; // -1 means not in a set

            else if (fromSet == toSet) return; // if already in the same set no need to continue

            foreach (var cell in sets[fromSet])
            {
                sets[toSet].Add(cell);
                cellSet[cell.y, cell.x] = toSet;
            }
            sets.Remove(fromSet);
        }

        private void AddToSet(int originalY, int originalX, int newY, int newX)
        {
            int setID = GetSetID(originalY, originalX);

            sets[setID].Add((newY, newX));
            cellSet[newY, newX] = setID;
        }

        private bool BoolInASet(int y, int x)
        {
            // -1 means not in a set so if it does not = -1 then it must be in a set
            return (GetSetID(y, x) != -1);
        }

        private bool BoolSameSet(int y1, int x1, int y2, int x2)
        {
            return (GetSetID(y1, x1) == GetSetID(y2, x2));
        }

        private int GetSetID(int y, int x)
        {
            return cellSet[y, x];
        }
    }
}
