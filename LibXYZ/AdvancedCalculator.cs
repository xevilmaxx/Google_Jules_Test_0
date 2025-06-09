using NLog;
using System;

namespace LibXYZ
{
    public class AdvancedCalculator
    {
        private readonly ILogger _logger;

        public AdvancedCalculator(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Info("AdvancedCalculator initialized.");
        }

        public double Add(double a, double b)
        {
            _logger.Debug($"Attempting to add {a} and {b}.");
            double result = a + b;
            _logger.Info($"Addition: {a} + {b} = {result}.");
            return result;
        }

        public double Subtract(double a, double b)
        {
            _logger.Debug($"Attempting to subtract {b} from {a}.");
            double result = a - b;
            _logger.Info($"Subtraction: {a} - {b} = {result}.");
            return result;
        }

        public double Multiply(double a, double b)
        {
            _logger.Debug($"Attempting to multiply {a} by {b}.");
            double result = a * b;
            _logger.Info($"Multiplication: {a} * {b} = {result}.");
            return result;
        }

        public double Divide(double a, double b)
        {
            _logger.Debug($"Attempting to divide {a} by {b}.");
            if (b == 0)
            {
                _logger.Error("Division by zero attempt.");
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            double result = a / b;
            _logger.Info($"Division: {a} / {b} = {result}.");
            return result;
        }
    }
}
