using System; // For ConsoleKey if we were to handle it here

namespace LibXYZ.PacMan
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Lives { get; set; }
        public int Score { get; set; }
        public char Symbol { get; private set; } = 'C';
        public Direction CurrentDirection { get; set; } = Direction.None;

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Lives = 3;
            Score = 0;
        }

        public void UpdatePosition(Board board)
        {
            if (board == null) return;

            int nextX = X;
            int nextY = Y;

            switch (CurrentDirection)
            {
                case Direction.Up:
                    nextY--;
                    break;
                case Direction.Down:
                    nextY++;
                    break;
                case Direction.Left:
                    nextX--;
                    break;
                case Direction.Right:
                    nextX++;
                    break;
                case Direction.None:
                    // No movement if direction is None
                    return;
            }

            // Boundary checks (optional, if board edges are not always walls)
            // if (nextX < 0 || nextX >= board.Width || nextY < 0 || nextY >= board.Height) return;

            if (!board.IsWall(nextX, nextY))
            {
                X = nextX;
                Y = nextY;
            }
        }
    }
}
