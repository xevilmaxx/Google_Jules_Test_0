using System; // For Random, ConsoleColor
using System.Collections.Generic; // For List
using System.Linq; // For LINQ methods like Any, Count, Where

namespace LibXYZ.PacMan
{
    public class Ghost
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }
        public Direction CurrentDirection { get; set; } = Direction.None;
        private static readonly Random _random = new Random();

        public Ghost(int startX, int startY, ConsoleColor color)
        {
            X = startX;
            Y = startY;
            Color = color;
            // Initial direction will be chosen by UpdatePosition's first call if None
        }

        public void UpdatePosition(Board board)
        {
            if (board == null) return;

            List<Direction> validDirections = GetValidDirections(board);

            if (!validDirections.Any())
            {
                // This case means the ghost is in a single isolated spot with walls all around.
                // Or, it's a bug in GetValidDirections or IsWall.
                // For now, do nothing if truly stuck (should not happen in a valid map).
                return;
            }

            // If current direction is None or not valid, pick a new random valid one.
            if (CurrentDirection == Direction.None || !validDirections.Contains(CurrentDirection))
            {
                CurrentDirection = validDirections[_random.Next(validDirections.Count)];
            }
            else // CurrentDirection is valid
            {
                // Try to avoid immediately reversing.
                Direction oppositeDirection = GetOppositeDirection(CurrentDirection);
                List<Direction> preferredDirections = validDirections.Where(d => d != oppositeDirection).ToList();

                if (preferredDirections.Any())
                {
                    // 70% chance to continue straight if possible
                    if (preferredDirections.Contains(CurrentDirection) && _random.Next(100) < 70)
                    {
                        // Keep CurrentDirection - no change needed here
                    }
                    else
                    {
                        // Pick a new direction from preferred ones
                        CurrentDirection = preferredDirections[_random.Next(preferredDirections.Count)];
                    }
                }
                else
                {
                    // Only option is to reverse (e.g., dead end)
                    CurrentDirection = oppositeDirection;
                }
            }

            // Move based on the chosen CurrentDirection
            switch (CurrentDirection)
            {
                case Direction.Up:    Y--; break;
                case Direction.Down:  Y++; break;
                case Direction.Left:  X--; break;
                case Direction.Right: X++; break;
            }
        }

        private List<Direction> GetValidDirections(Board board)
        {
            List<Direction> directions = new List<Direction>();
            if (CanMove(board, Direction.Up)) directions.Add(Direction.Up);
            if (CanMove(board, Direction.Down)) directions.Add(Direction.Down);
            if (CanMove(board, Direction.Left)) directions.Add(Direction.Left);
            if (CanMove(board, Direction.Right)) directions.Add(Direction.Right);
            return directions;
        }

        private bool CanMove(Board board, Direction direction)
        {
            int nextX = X, nextY = Y;
            switch (direction)
            {
                case Direction.Up:    nextY--; break;
                case Direction.Down:  nextY++; break;
                case Direction.Left:  nextX--; break;
                case Direction.Right: nextX++; break;
                case Direction.None: return false; // Cannot move in no direction
            }
            return !board.IsWall(nextX, nextY);
        }

        private Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:    return Direction.Down;
                case Direction.Down:  return Direction.Up;
                case Direction.Left:  return Direction.Right;
                case Direction.Right: return Direction.Left;
                default:              return Direction.None; // Or throw an exception for invalid input
            }
        }
    }
}
