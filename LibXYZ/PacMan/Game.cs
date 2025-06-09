using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LibXYZ.PacMan
{
    public class Game
    {
        private readonly ILogger _logger;
        private Board _board;
        private Player _player;
        private List<Ghost> _ghosts;
        private GameRenderer _renderer;
        private Difficulty _selectedDifficulty;
        private int _playerStartX, _playerStartY;

        public Game(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Info("PacMan Game engine initialized.");
            _board = null!;
            _player = null!;
            _renderer = null!;
            _ghosts = new List<Ghost>();
        }

        private (int startX, int startY) FindCharacterStartPosition(Board board, char charToFind)
        {
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    if (board.GetCell(x,y) == charToFind)
                    {
                        return (x,y);
                    }
                }
            }
            _logger.Warn($"Character '{charToFind}' start symbol not found. Defaulting.");
            return charToFind == 'P' ? (1,1) : (board.Width / 2, board.Height / 2);
        }

        private void InitializeGhosts()
        {
            _ghosts.Clear();
            _ghosts.Add(new Ghost(9, 7, ConsoleColor.Red));    // Blinky
            _ghosts.Add(new Ghost(10, 7, ConsoleColor.Magenta)); // Pinky
            _ghosts.Add(new Ghost(11, 7, ConsoleColor.Cyan));   // Inky
            _logger.Info($"Initialized {_ghosts.Count} ghosts.");
        }

        public void Start()
        {
            Console.Clear();
            // Welcome message is quickly cleared by difficulty selection, then game render.
            // Console.WriteLine("Welcome to Pac-Man (Text Edition)!");
            _logger.Info("Pac-Man game Start() method invoked.");
            Console.CursorVisible = false;

            Console.SetCursorPosition(0,1);
            Console.WriteLine("Select difficulty:");
            Console.WriteLine("1: Easy");
            Console.WriteLine("2: Medium");
            Console.WriteLine("3: Hard");
            Console.Write("Enter choice (1-3): ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1": _selectedDifficulty = Difficulty.Easy; break;
                case "2": _selectedDifficulty = Difficulty.Medium; break;
                case "3": _selectedDifficulty = Difficulty.Hard; break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to Easy.");
                    _selectedDifficulty = Difficulty.Easy;
                    break;
            }
            _logger.Info($"Difficulty selected: {_selectedDifficulty}");

            _board = new Board(_selectedDifficulty);
            (_playerStartX, _playerStartY) = FindCharacterStartPosition(_board, 'P');
            _player = new Player(_playerStartX, _playerStartY);
            _renderer = new GameRenderer();
            InitializeGhosts();

            bool gameRunning = true;
            string gameEndMessage = "Game Over!";

            while (gameRunning)
            {
                // 1. Handle Input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        gameRunning = false;
                        gameEndMessage = "Game manually quit.";
                        _logger.Info("Game quit by player.");
                        break;
                    }
                    Direction newDirection = _player.CurrentDirection;
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:    newDirection = Direction.Up;    break;
                        case ConsoleKey.DownArrow:  newDirection = Direction.Down;  break;
                        case ConsoleKey.LeftArrow:  newDirection = Direction.Left;  break;
                        case ConsoleKey.RightArrow: newDirection = Direction.Right; break;
                    }
                    if (newDirection != _player.CurrentDirection || _player.CurrentDirection == Direction.None)
                    {
                         _player.CurrentDirection = newDirection;
                    }
                }

                // 2. Update Game State
                _player.UpdatePosition(_board);

                // Pellet collection
                char cellContent = _board.GetCell(_player.X, _player.Y);
                if (cellContent == '.')
                {
                    _player.Score += 10;
                    _board.SetCell(_player.X, _player.Y, ' ');
                    _board.DecrementPelletCount();
                }
                else if (cellContent == '*')
                {
                    _player.Score += 50;
                    _board.SetCell(_player.X, _player.Y, ' ');
                    _board.DecrementPelletCount();
                    // TODO: Power-up logic
                }

                // Move Ghosts
                foreach (var ghost in _ghosts)
                {
                    ghost.UpdatePosition(_board);
                }

                // Collision Detection (Pac-Man vs Ghosts)
                foreach (var ghost in _ghosts)
                {
                    if (_player.X == ghost.X && _player.Y == ghost.Y)
                    {
                        _player.Lives--;
                        _logger.Info($"Pac-Man collided with a ghost at ({_player.X},{_player.Y})! Lives left: {_player.Lives}");

                        if (_player.Lives <= 0)
                        {
                            gameRunning = false;
                            gameEndMessage = "Caught by a ghost! GAME OVER";
                            _logger.Info("Game Over - Pac-Man ran out of lives.");
                            break; // Exit ghost collision loop
                        }
                        else
                        {
                            // Reset player to start position
                            _player.X = _playerStartX;
                            _player.Y = _playerStartY;
                            _player.CurrentDirection = Direction.None; // Stop player movement
                             _logger.Info($"Player reset to start position ({_playerStartX},{_playerStartY}).");
                            Thread.Sleep(500); // Brief pause after losing a life
                        }
                    }
                }
                if (!gameRunning) break; // Exit main loop if game over from collision

                // Check Win Condition (all pellets eaten)
                if (_board.PelletCount == 0)
                {
                    gameRunning = false;
                    gameEndMessage = "You cleared the level! YOU WIN!";
                    _logger.Info("Level cleared! All pellets eaten.");
                }

                // 3. Render Game
                _renderer.ClearScreen(_board.Width, _board.Height + 2);
                _renderer.DrawBoard(_board);
                _renderer.DrawPlayer(_player.X, _player.Y, _player.Symbol);
                foreach (var ghost in _ghosts) // Render ghosts
                {
                    _renderer.DrawGhost(ghost.X, ghost.Y, 'G', ghost.Color); // Assuming 'G' for ghost symbol
                }
                _renderer.DrawScore(_player.Score, _player.Lives, _board.Height);

                Thread.Sleep(150);
            }

            _renderer.DisplayMessage($"{gameEndMessage} Final Score: {_player.Score}. Press any key to return to menu.", _board.Height, 2);
            Console.ReadKey(true);
            Console.CursorVisible = true;
        }
    }
}
