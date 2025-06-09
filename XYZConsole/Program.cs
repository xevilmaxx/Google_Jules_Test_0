using NLog;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using LibXYZ;
using LibXYZ.PacMan;
using LibXYZ.Database;

namespace XYZConsole
{
    class Program
    {
        private static ILogger? logger;
        private static AdvancedCalculator? calculator;

        static void Main(string[] args)
        {
            try { LogManager.Setup().LoadConfigurationFromFile("nlog.config"); logger = LogManager.GetCurrentClassLogger(); }
            catch (Exception ex) { Console.WriteLine($"NLog init error: {ex.Message}"); }
            logger?.Info("XYZConsole application started.");
            LocalizationManager.SetLanguage("en");
            if (logger != null) calculator = new AdvancedCalculator(logger);
            else Console.WriteLine(LocalizationManager.GetString("CriticalErrorLoggerNotInitialized"));
            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n" + LocalizationManager.GetString("MenuPrompt")); // MenuPrompt updated in resx
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // Perform Calculation
                        if (calculator != null) PerformCalculation();
                        else Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized"));
                        break;
                    case "2": // Change Language
                        ChangeLanguage();
                        break;
                    case "3": // Play Pac-Man
                        if (logger != null)
                        {
                            logger.Info("Starting Pac-Man game from console menu.");
                            LibXYZ.PacMan.Game pacManGame = new LibXYZ.PacMan.Game(logger);
                            pacManGame.Start();
                            Console.Clear();
                            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));
                        }
                        else Console.WriteLine(LocalizationManager.GetString("CriticalErrorLoggerNotInitialized"));
                        break;
                    case "4": // Placeholder / Not Implemented
                        Console.WriteLine(LocalizationManager.GetString("OptionNotImplemented"));
                        Console.WriteLine("(Press any key to continue)");
                        Console.ReadLine(); // Changed for automated testing
                        break;
                    case "5": // Database Operations (New position)
                        HandleDatabaseOperations();
                        break;
                    case "6": // Exit (New position)
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
                case "1": LocalizationManager.SetLanguage("en"); logger?.Info("Language changed to English."); break;
                case "2": LocalizationManager.SetLanguage("es"); logger?.Info("Language changed to Spanish."); break;
                default: Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); break;
            }
            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));
        }

        static void PerformCalculation()
        {
            if (calculator == null) { Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized")); return; }
            double num1, num2; string? operation;
            try
            {
                Console.WriteLine(LocalizationManager.GetString("EnterFirstNumberPrompt"));
                if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.CurrentCulture, out num1)) { Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); return; }
                Console.WriteLine(LocalizationManager.GetString("EnterOperationPrompt"));
                operation = Console.ReadLine();
                Console.WriteLine(LocalizationManager.GetString("EnterSecondNumberPrompt"));
                if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.CurrentCulture, out num2)) { Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); return; }
                double result;
                switch (operation)
                {
                    case "+": result = calculator.Add(num1, num2); break;
                    case "-": result = calculator.Subtract(num1, num2); break;
                    case "*": result = calculator.Multiply(num1, num2); break;
                    case "/": result = calculator.Divide(num1, num2); break;
                    default: Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); return;
                }
                Console.WriteLine($"{LocalizationManager.GetString("ResultPrefix")} {result}");
            }
            catch (DivideByZeroException) { Console.WriteLine(LocalizationManager.GetString("DivisionByZeroError")); }
            catch (Exception ex) { logger?.Error(ex, "Error during calculation."); Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); }
        }

        static void HandleDatabaseOperations()
        {
            logger?.Info("Handling Database Operations.");
            LogService logService = new LogService();

            Console.WriteLine($"--- {LocalizationManager.GetString("ViewLogEntriesPrompt")} ---");
            List<LogEntry> entries = logService.GetAllLogEntries();
            if (entries.Count == 0)
            {
                Console.WriteLine(LocalizationManager.GetString("NoEntriesFoundMessage"));
            }
            else
            {
                foreach (var entry in entries)
                {
                    // Formatting DateTime for consistent output
                    Console.WriteLine($"ID: {entry.Id}, Time: {entry.Timestamp:yyyy-MM-dd HH:mm:ss UTC}, Msg: \"{entry.Message}\", Val: {entry.RandomValue}");
                }
            }
            Console.WriteLine("--------------------");

            Console.Write(LocalizationManager.GetString("AddRandomEntryPrompt"));
            string? addChoice = Console.ReadLine()?.ToLower();
            // Use GetString("YesChar").ToLower() for robust comparison with localized 'y' or 's'
            if (addChoice == LocalizationManager.GetString("YesChar").ToLower())
            {
                logService.AddRandomLogEntry();
                Console.WriteLine(LocalizationManager.GetString("EntryAddedMessage"));

                Console.WriteLine($"--- {LocalizationManager.GetString("ViewLogEntriesPrompt")} (Updated) ---");
                entries = logService.GetAllLogEntries();
                if (entries.Count == 0)
                {
                     Console.WriteLine(LocalizationManager.GetString("NoEntriesFoundMessage"));
                }
                else
                {
                    foreach (var entry in entries)
                    {
                        Console.WriteLine($"ID: {entry.Id}, Time: {entry.Timestamp:yyyy-MM-dd HH:mm:ss UTC}, Msg: \"{entry.Message}\", Val: {entry.RandomValue}");
                    }
                }
                Console.WriteLine("--------------------");
            }
            Console.WriteLine("(Press any key to return to main menu)");
            Console.ReadLine(); // Changed for automated testing
        }
    }
}
