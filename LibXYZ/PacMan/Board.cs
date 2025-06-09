using System; // For Math.Max

namespace LibXYZ.PacMan
{
    public class Board
    {
        private char[,] _layout;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int PelletCount { get; private set; }

        public Board(Difficulty difficulty)
        {
            // Dimensions can be based on difficulty later
            // For now, a fixed small level for testing
            InitializeSimpleLevel();
        }

        private void InitializeSimpleLevel()
        {
            // Dimensions for a small test level
            Width = 20; // Example width
            // Height = 10; // Example height, will be overwritten by simpleLevel.Length
            _layout = new char[Height, Width]; // [rows, columns] which is [y, x]
            PelletCount = 0;

            string[] simpleLevel = new string[]
            {
                "####################",
                "#P........#........#",
                "#.###.###.#.###.###.",
                "#.# #.# #.#.# #.# #*",
                "#.###.###.#.###.###.",
                "#.........#........#",
                "#.###.#.#####.#.###.",
                "#.....#...#...#....#",
                "####################",
            };
            // Ensure level matches Height. Add/remove rows if simpleLevel definition changes.
            Height = simpleLevel.Length; // Set Height based on actual level definition
            _layout = new char[Height, Width]; // Re-initialize _layout with correct Height


            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (y < simpleLevel.Length && x < simpleLevel[y].Length)
                    {
                        char cell = simpleLevel[y][x];
                        _layout[y, x] = cell;
                        if (cell == '.')
                        {
                            PelletCount++;
                        }
                        else if (cell == '*') // Power pellet
                        {
                            PelletCount++; // Also counts as a pellet to be eaten
                        }
                        // 'P' can be a starting position, not a wall or pellet. Handled by Game.cs.
                        // For now, treat 'P' as an empty space after player is placed.
                        if (cell == 'P')
                        {
                             _layout[y,x] = ' '; // Treat 'P' as empty space on board
                        }
                    }
                    else
                    {
                        _layout[y, x] = ' '; // Fill any remaining space if level smaller than Width/Height
                    }
                }
            }
        }

        public char GetCell(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return _layout[y, x];
            }
            return '#'; // Treat out of bounds as a wall
        }

        public void SetCell(int x, int y, char value)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                // If a pellet is removed, decrement count
                // This logic is better handled in Game.cs when player eats a pellet
                // char oldCell = _layout[y,x];
                // if((oldCell == '.' || oldCell == '*') && value == ' ') PelletCount = Math.Max(0, PelletCount -1);
                _layout[y, x] = value;
            }
        }

        public void DecrementPelletCount()
        {
            PelletCount = Math.Max(0, PelletCount - 1);
        }

        public bool IsWall(int x, int y)
        {
            return GetCell(x, y) == '#';
        }
    }
}
