using System; // For ConsoleKey, if it were used here. Not strictly needed for current placeholder.

namespace LibXYZ.PacMan
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Lives { get; set; }
        public int Score { get; set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Lives = 3;
            Score = 0;
        }

        public void Move(ConsoleKey key, Board board)
        {
            // Placeholder for movement logic
        }
    }
}
