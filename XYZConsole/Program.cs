using NLog;
using System;
using System.Globalization; // Required for CultureInfo
using System.Threading;    // Required for Thread.CurrentThread
using LibXYZ;

namespace XYZConsole
{
    class Program
    {
        private static ILogger? logger; // Nullable logger
        private static AdvancedCalculator? calculator;

        static void Main(string[] args)
        {
            // Initialize NLog first
            try
            {
                LogManager.Setup().LoadConfigurationFromFile("nlog.config");
                logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical NLog initialization error: {ex.Message}. Logging will be unavailable.");
                // Optionally, exit if logging is absolutely critical
            }

            logger?.Info("XYZConsole application started.");

            // Initialize LocalizationManager - default to English
            LocalizationManager.SetLanguage("en");

            // Initialize Calculator
            if (logger != null)
            {
                calculator = new AdvancedCalculator(logger);
            }
            else
            {
                // If logger failed, we can't create calculator.
                // Program will still run but calculations will be disabled in the menu.
                Console.WriteLine(LocalizationManager.GetString("CriticalErrorLoggerNotInitialized"));
            }

            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));

            bool running = true;
            while (running)
            {
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
                             Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized")); // Create this key
                        }
                        break;
                    case "2": // Change Language
                        ChangeLanguage();
                        break;
                    case "3": // Exit
                        running = false;
                        break;
                    default:
                        Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                        break;
                }
            }

            Console.WriteLine(LocalizationManager.GetString("GoodbyeMessage"));
            logger?.Info("XYZConsole application finished successfully.");
            LogManager.Shutdown(); // Flush and shutdown NLog
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
            // Re-display welcome message in new language
            Console.WriteLine(LocalizationManager.GetString("WelcomeMessage"));
        }

        static void PerformCalculation()
        {
            if (calculator == null) // Should be checked before calling
            {
                Console.WriteLine(LocalizationManager.GetString("CriticalErrorCalculatorNotInitialized")); // Create this key
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
                        result = calculator.Divide(num1, num2); // Handles DivideByZeroException internally
                        break;
                    default:
                        Console.WriteLine(LocalizationManager.GetString("InvalidInputError"));
                        return;
                }
                Console.WriteLine($"{LocalizationManager.GetString("ResultPrefix")} {result}");
            }
            catch (DivideByZeroException)
            {
                // AdvancedCalculator already logs this error. We just show localized message.
                Console.WriteLine(LocalizationManager.GetString("DivisionByZeroError"));
            }
            catch (Exception ex)
            {
                logger?.Error(ex, "Error during calculation.");
                Console.WriteLine(LocalizationManager.GetString("InvalidInputError")); // Generic error for other calculation issues
            }
        }
    }
}
