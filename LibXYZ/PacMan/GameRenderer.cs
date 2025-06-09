using System; // For Console
using System.Collections.Generic; // For List

namespace LibXYZ.PacMan
{
    public class GameRenderer
    {
        public void Draw(Board board, Player player, List<Ghost> ghosts, int score, int lives)
        {
            // Placeholder for console drawing logic
            // Example:
            // Console.Clear();
            // Draw board layout (walls, pellets)
            // Draw player
            // Draw ghosts
            // Draw score and lives
            // Console.SetCursorPosition(0, board.Height + 1); // Move cursor below board
            // Console.WriteLine($"Score: {score}  Lives: {lives}");
        }

        public void DisplayMessage(string message, int yOffset = 0)
        {
            // Utility to display messages at a specific console line
            // Console.SetCursorPosition(0, yOffset);
            // Console.WriteLine(message);
        }
    }
}
