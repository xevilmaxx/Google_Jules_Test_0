using Xunit;
using LibXYZ;
using NLog;
using System;

namespace LibXYZ.Tests
{
    public class AdvancedCalculatorTests
    {
        private readonly AdvancedCalculator _calculator;
        private readonly ILogger _logger;

        public AdvancedCalculatorTests()
        {
            // Configure NLog for tests (reads LibXYZ.Tests/nlog.config)
            // Ensure nlog.config is set to 'Copy to Output Directory'
            try
            {
                LogManager.Setup().LoadConfigurationFromFile("nlog.config");
            }
            catch (System.IO.FileNotFoundException)
            {
                // Fallback or error if nlog.config is not found - for tests, we might not always need file logging
                // For simplicity in this subtask, we'll proceed, but a real scenario might need robust handling
                Console.WriteLine("nlog.config not found for tests. Console logging might be limited.");
            }
            _logger = LogManager.GetCurrentClassLogger();
            _calculator = new AdvancedCalculator(_logger);
        }

        [Theory]
        [InlineData(5, 3, 8)]
        [InlineData(-5, 3, -2)]
        [InlineData(0, 0, 0)]
        [InlineData(10, -5, 5)]
        [InlineData(-5, -5, -10)]
        public void Add_ShouldReturnCorrectSum(double a, double b, double expected)
        {
            _logger.Debug($"Test: Add({a}, {b})");
            double result = _calculator.Add(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 3, 2)]
        [InlineData(-5, 3, -8)]
        [InlineData(0, 0, 0)]
        [InlineData(10, -5, 15)]
        [InlineData(-5, -5, 0)]
        public void Subtract_ShouldReturnCorrectDifference(double a, double b, double expected)
        {
            _logger.Debug($"Test: Subtract({a}, {b})");
            double result = _calculator.Subtract(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, 3, 15)]
        [InlineData(-5, 3, -15)]
        [InlineData(0, 5, 0)]
        [InlineData(10, 0, 0)]
        [InlineData(-5, -5, 25)]
        public void Multiply_ShouldReturnCorrectProduct(double a, double b, double expected)
        {
            _logger.Debug($"Test: Multiply({a}, {b})");
            double result = _calculator.Multiply(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 2, 5)]
        [InlineData(-10, 2, -5)]
        [InlineData(0, 5, 0)]
        [InlineData(10, -2, -5)]
        [InlineData(5, 2, 2.5)]
        public void Divide_ShouldReturnCorrectQuotient(double a, double b, double expected)
        {
            _logger.Debug($"Test: Divide({a}, {b})");
            double result = _calculator.Divide(a, b);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Divide_ByZero_ShouldThrowDivideByZeroException()
        {
            _logger.Debug("Test: Divide(10, 0) for exception");
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
        }
    }
}
