using System;
using System.Collections.Generic; // Required for List<Ghost> if used in future Draw method

namespace LibXYZ.PacMan
{
    public class GameRenderer
    {
        public void DrawBoard(Board board)
        {
            if (board == null) return;
            for (int y = 0; y < board.Height; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < board.Width; x++)
                {
                    Console.Write(board.GetCell(x, y));
                }
            }
        }

        // Player and Ghost will be passed in later steps
        public void DrawPlayer(int x, int y, char playerSymbol)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(playerSymbol);
        }

        public void DrawGhost(int x, int y, char ghostSymbol, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(ghostSymbol);
            Console.ResetColor();
        }

        public void DrawScore(int score, int lives, int boardHeight)
        {
            Console.SetCursorPosition(0, boardHeight); // Position below the board
            Console.WriteLine($"Score: {score}  Lives: {lives}    "); // Extra spaces to clear previous longer score
        }

        public void DisplayMessage(string message, int boardHeight, int lineOffset = 1)
        {
            Console.SetCursorPosition(0, boardHeight + lineOffset);
            // Clear the line first
            Console.Write(new string(' ', Console.WindowWidth > 0 ? Console.WindowWidth -1 : 80));
            Console.SetCursorPosition(0, boardHeight + lineOffset);
            Console.WriteLine(message);
        }

        public void ClearScreen(int width, int height)
        {
            // A more controlled clear than Console.Clear() for the game area
            for (int y = 0; y < height + 5; ++y) // Clear a few extra lines for messages
            {
                Console.SetCursorPosition(0,y);
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0,0); // Reset cursor
        }
    }
}
