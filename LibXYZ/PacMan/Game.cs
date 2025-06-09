using NLog; // For ILogger
using System; // For Console, Clear, ReadKey, etc.
using System.Collections.Generic; // For List

namespace LibXYZ.PacMan
{
    public class Game
    {
        private readonly ILogger _logger;
        private Board? _board;
        private Player? _player;
        private List<Ghost>? _ghosts;
        private GameRenderer? _renderer;
        private Difficulty _selectedDifficulty;

        public Game(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Info("PacMan Game engine initialized.");
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Pac-Man (Text Edition)!");
            _logger.Info("Pac-Man game Start() method invoked.");

            Console.WriteLine("Select difficulty:");
            Console.WriteLine("1: Easy");
            Console.WriteLine("2: Medium");
            Console.WriteLine("3: Hard");
            Console.Write("Enter choice (1-3): ");

            string? input = Console.ReadLine();
            bool difficultyChosen = false; // Though current logic always sets it true after input block

            if (input != null)
            {
                switch (input)
                {
                    case "1":
                        _selectedDifficulty = Difficulty.Easy;
                        difficultyChosen = true;
                        break;
                    case "2":
                        _selectedDifficulty = Difficulty.Medium;
                        difficultyChosen = true;
                        break;
                    case "3":
                        _selectedDifficulty = Difficulty.Hard;
                        difficultyChosen = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Defaulting to Easy.");
                        _selectedDifficulty = Difficulty.Easy; // Default on invalid input
                        difficultyChosen = true;
                        break;
                }
            }
            else
            {
                 Console.WriteLine("No input. Defaulting to Easy.");
                _selectedDifficulty = Difficulty.Easy; // Default on null input
                difficultyChosen = true;
            }

            if (difficultyChosen)
            {
                 _logger.Info($"Difficulty selected: {_selectedDifficulty}");
                 Console.WriteLine($"Pac-Man game starting with {_selectedDifficulty} difficulty...");
                 // Initialize other game components here based on difficulty
                 // _board = new Board(_selectedDifficulty);
                 // _player = new Player(...);
                 // _ghosts = new List<Ghost>(); ...
                 // _renderer = new GameRenderer();
            }

            Console.WriteLine("(Press any key to return to menu)");
            Console.ReadLine(); // Changed from ReadKey() to allow piped input for smoke test
        }
    }
}
