using NLog;
using System;
using LibXYZ; // Reference to the class library

namespace XYZConsole
{
    class Program
    {
        private static ILogger? logger; // Nullable logger

        static void Main(string[] args)
        {
            try
            {
                // Configure NLog. This will automatically load nlog.config
                LogManager.Setup().LoadConfigurationFromFile("nlog.config");
                logger = LogManager.GetCurrentClassLogger();

                logger.Info("XYZConsole application started.");

                // Ensure logger is not null before using it
                if (logger == null)
                {
                    Console.WriteLine("Error: Logger could not be initialized.");
                    return;
                }

                AdvancedCalculator calculator = new AdvancedCalculator(logger);

                logger.Debug("Performing calculations...");

                double sum = calculator.Add(10, 5);
                Console.WriteLine($"10 + 5 = {sum}");

                double difference = calculator.Subtract(10, 5);
                Console.WriteLine($"10 - 5 = {difference}");

                double product = calculator.Multiply(10, 5);
                Console.WriteLine($"10 * 5 = {product}");

                double quotient = calculator.Divide(10, 5);
                Console.WriteLine($"10 / 5 = {quotient}");

                logger.Debug("Attempting division by zero.");
                try
                {
                    calculator.Divide(10, 0);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    // The error is already logged by the calculator class
                }

                logger.Info("XYZConsole application finished successfully.");
            }
            catch (Exception ex)
            {
                // Log any unexpected exception
                if (logger != null)
                {
                    logger.Fatal(ex, "An unhandled exception occurred in XYZConsole.");
                }
                else
                {
                    Console.WriteLine($"Critical Error: {ex.Message}. Logger was not available.");
                }
            }
            finally
            {
                // Ensure to flush and shutdown NLog, otherwise logs might be lost
                LogManager.Shutdown();
            }
        }
    }
}
