using NLog;
using System;
using System.Globalization;
using System.Threading;
using LibXYZ; // For LocalizationManager and PacMan
using LibXYZ.PacMan; // For Game specifically

namespace XYZConsole
{
    class Program
    {
        private static ILogger? logger;
        private static AdvancedCalculator? calculator;

        static void Main(string[] args)
        {
            try
            {
                LogManager.Setup().LoadConfigurationFromFile("nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical NLog initialization error: {ex.Message}. Logging will be unavailable.");
            }

            logger?.Info("XYZConsole application started.");
            LocalizationManager.SetLanguage("en");

            if (logger != null)
            {
                calculator = new AdvancedCalculator(logger);
            }
            else
            {
                Console.WriteLine(LocalizationManager.GetString("CriticalErrorLoggerNotInitialized"));
            }

            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));

            bool running = true;
            while (running)
            {
                // MenuPrompt now includes all options up to Exit.
                Console.WriteLine("\n" + LocalizationManager.GetString("MenuPrompt"));
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Perform Calculation
                        if (calculator != null)
                        {
                            PerformCalculation();
                        }
                        else
                        {
                             Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized"));
                        }
                        break;
                    case "2": // Change Language
                        ChangeLanguage();
                        break;
                    case "3": // Play Pac-Man (New)
                        if (logger != null)
                        {
                            logger.Info("Starting Pac-Man game from console menu.");
                            LibXYZ.PacMan.Game pacManGame = new LibXYZ.PacMan.Game(logger);
                            pacManGame.Start();
                            // After PacMan finishes, re-display main welcome/menu context
                            Console.Clear(); // Clear PacMan screen
                            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage")); // Re-show welcome
                        }
                        else
                        {
                            Console.WriteLine(LocalizationManager.GetString("CriticalErrorLoggerNotInitialized"));
                        }
                        break;
                    case "4": // Exit (was 3)
                        running = false;
                        break;
                    default:
                        Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                        break;
                }
            }

            Console.WriteLine(LocalizationManager.GetString("GoodbyeMessage"));
            logger?.Info("XYZConsole application finished successfully.");
            LogManager.Shutdown();
        }

        static void ChangeLanguage()
        {
            Console.WriteLine(LocalizationManager.GetString("SelectLanguagePrompt"));
            string? langChoice = Console.ReadLine();
            switch (langChoice)
            {
                case "1":
                    LocalizationManager.SetLanguage("en");
                    logger?.Info("Language changed to English.");
                    break;
                case "2":
                    LocalizationManager.SetLanguage("es");
                    logger?.Info("Language changed to Spanish.");
                    break;
                default:
                    Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                    break;
            }
            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));
        }

        static void PerformCalculation()
        {
            if (calculator == null)
            {
                Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized"));
                return;
            }

            double num1, num2;
            string? operation;

            try
            {
                Console.WriteLine(LocalizationManager.GetString("EnterFirstNumberPrompt"));
                string? input1 = Console.ReadLine();
                if (!double.TryParse(input1, NumberStyles.Any, CultureInfo.CurrentCulture, out num1))
                {
                    Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                    return;
                }

                Console.WriteLine(LocalizationManager.GetString("EnterOperationPrompt"));
                operation = Console.ReadLine();

                Console.WriteLine(LocalizationManager.GetString("EnterSecondNumberPrompt"));
                string? input2 = Console.ReadLine();
                if (!double.TryParse(input2, NumberStyles.Any, CultureInfo.CurrentCulture, out num2))
                {
                    Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                    return;
                }

                double result;
                switch (operation)
                {
                    case "+":
                        result = calculator.Add(num1, num2);
                        break;
                    case "-":
                        result = calculator.Subtract(num1, num2);
                        break;
                    case "*":
                        result = calculator.Multiply(num1, num2);
                        break;
                    case "/":
                        result = calculator.Divide(num1, num2);
                        break;
                    default:
                        Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                        return;
                }
                Console.WriteLine($"{LocalizationManager.GetString("ResultPrefix")} {result}");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine(LocalizationManager.GetString("DivisionByZeroError"));
            }
            catch (Exception ex)
            {
                logger?.Error(ex, "Error during calculation.");
                Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
            }
        }
    }
}
