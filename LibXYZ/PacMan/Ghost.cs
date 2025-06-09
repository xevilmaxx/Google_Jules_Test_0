using System; // For ConsoleColor

namespace LibXYZ.PacMan
{
    public class Ghost
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; } // For text-based rendering

        public Ghost(int startX, int startY, ConsoleColor color)
        {
            X = startX;
            Y = startY;
            Color = color;
        }

        public virtual void Move(Board board, Player player)
        {
            // Placeholder for AI movement logic
        }
    }
}
