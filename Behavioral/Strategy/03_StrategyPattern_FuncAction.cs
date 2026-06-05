// Strategy Pattern: Func and Action Approach
// Best for: Simple, concise strategies with maximum use of lambda expressions
// Func<T, TResult> - for strategies that return a value
// Action<T> - for strategies that return void

using System;
using System.Collections.Generic;

namespace StrategyPatternExamples
{
    // Example 1: Simple Payment Processing with Action<decimal>
    public class SimplePaymentProcessor
    {
        private Action<decimal> _paymentStrategy;

        public SimplePaymentProcessor(Action<decimal> paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void SetPaymentStrategy(Action<decimal> paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void ProcessPayment(decimal amount)
        {
            _paymentStrategy(amount);
        }
    }

    // Example 2: Discount Calculation with Func<decimal, decimal>
    public class DiscountCalculator
    {
        private Func<decimal, decimal> _discountStrategy;
        private readonly string _strategyName;

        public DiscountCalculator(string strategyName, Func<decimal, decimal> discountStrategy)
        {
            _strategyName = strategyName;
            _discountStrategy = discountStrategy;
        }

        public void SetDiscountStrategy(string strategyName, Func<decimal, decimal> discountStrategy)
        {
            _strategyName = strategyName;
            _discountStrategy = discountStrategy;
        }

        public decimal CalculatePrice(decimal originalPrice)
        {
            decimal finalPrice = _discountStrategy(originalPrice);
            Console.WriteLine($"Original Price: ${originalPrice:F2}");
            Console.WriteLine($"Strategy: {_strategyName}");
            Console.WriteLine($"Final Price: ${finalPrice:F2}");
            Console.WriteLine($"Savings: ${originalPrice - finalPrice:F2}\n");
            return finalPrice;
        }
    }

    // Example 3: Sorting with Action<int[]>
    public class SortingOrchestrator
    {
        private Action<int[]> _sortStrategy;
        private readonly string _strategyName;

        public SortingOrchestrator(string strategyName, Action<int[]> sortStrategy)
        {
            _strategyName = strategyName;
            _sortStrategy = sortStrategy;
        }

        public void SetSortStrategy(string strategyName, Action<int[]> sortStrategy)
        {
            _strategyName = strategyName;
            _sortStrategy = sortStrategy;
        }

        public void Sort(int[] data)
        {
            Console.WriteLine($"Original Array: [{string.Join(", ", data)}]");
            Console.WriteLine($"Sorting Strategy: {_strategyName}");
            _sortStrategy(data);
            Console.WriteLine($"Sorted Array: [{string.Join(", ", data)}]\n");
        }
    }

    // Example 4: Complex Strategy with Multiple Parameters
    public class DataProcessor
    {
        private Func<List<int>, int> _processingStrategy;
        private readonly string _strategyName;

        public DataProcessor(string strategyName, Func<List<int>, int> processingStrategy)
        {
            _strategyName = strategyName;
            _processingStrategy = processingStrategy;
        }

        public void SetProcessingStrategy(string strategyName, Func<List<int>, int> processingStrategy)
        {
            _strategyName = strategyName;
            _processingStrategy = processingStrategy;
        }

        public int Process(List<int> data)
        {
            Console.WriteLine($"Data: [{string.Join(", ", data)}]");
            Console.WriteLine($"Processing Strategy: {_strategyName}");
            int result = _processingStrategy(data);
            Console.WriteLine($"Result: {result}\n");
            return result;
        }
    }

    // Example 5: String Transformation with Func<string, string>
    public class TextTransformer
    {
        private Func<string, string> _transformStrategy;
        private readonly string _strategyName;

        public TextTransformer(string strategyName, Func<string, string> transformStrategy)
        {
            _strategyName = strategyName;
            _transformStrategy = transformStrategy;
        }

        public void SetTransformStrategy(string strategyName, Func<string, string> transformStrategy)
        {
            _strategyName = strategyName;
            _transformStrategy = transformStrategy;
        }

        public string Transform(string input)
        {
            Console.WriteLine($"Input: \"{input}\"");
            Console.WriteLine($"Strategy: {_strategyName}");
            string result = _transformStrategy(input);
            Console.WriteLine($"Output: \"{result}\"\n");
            return result;
        }
    }

    // Demo
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Strategy Pattern: Func and Action Approach ===\n");

            // Example 1: Simple Payment Processing with Action
            Console.WriteLine("--- Example 1: Payment Processing with Action<decimal> ---");
            var paymentProcessor = new SimplePaymentProcessor(
                amount => Console.WriteLine($"Processing credit card payment of ${amount}\n")
            );
            paymentProcessor.ProcessPayment(100m);

            paymentProcessor.SetPaymentStrategy(
                amount => Console.WriteLine($"Processing PayPal payment of ${amount}\n")
            );
            paymentProcessor.ProcessPayment(50m);

            // Example 2: Discount Calculation with Func
            Console.WriteLine("--- Example 2: Discount Calculation with Func<decimal, decimal> ---");
            var calculator = new DiscountCalculator(
                "No Discount",
                price => price
            );
            calculator.CalculatePrice(100m);

            calculator.SetDiscountStrategy(
                "10% Discount",
                price => price * 0.9m
            );
            calculator.CalculatePrice(100m);

            calculator.SetDiscountStrategy(
                "Volume Discount (20% off if > $150)",
                price => price > 150 ? price * 0.8m : price
            );
            calculator.CalculatePrice(200m);

            // Example 3: Sorting with Action
            Console.WriteLine("--- Example 3: Sorting Strategies with Action<int[]> ---");
            var sorter = new SortingOrchestrator(
                "Array.Sort (Built-in)",
                arr => Array.Sort(arr)
            );
            int[] data1 = { 5, 2, 8, 1, 9 };
            sorter.Sort(data1);

            sorter.SetSortStrategy(
                "Reverse Sort",
                arr => Array.Sort(arr, (a, b) => b.CompareTo(a))
            );
            int[] data2 = { 5, 2, 8, 1, 9 };
            sorter.Sort(data2);

            // Example 4: Data Processing with Complex Func
            Console.WriteLine("--- Example 4: Data Processing with Func<List<int>, int> ---");
            var processor = new DataProcessor(
                "Sum",
                data => data.Count > 0 ? data[0] : 0
            );
            processor.SetProcessingStrategy(
                "Sum",
                data =>
                {
                    int sum = 0;
                    foreach (int num in data)
                        sum += num;
                    return sum;
                }
            );
            processor.Process(new List<int> { 10, 20, 30, 40 });

            processor.SetProcessingStrategy(
                "Average",
                data =>
                {
                    int sum = 0;
                    foreach (int num in data)
                        sum += num;
                    return sum / data.Count;
                }
            );
            processor.Process(new List<int> { 10, 20, 30, 40 });

            processor.SetProcessingStrategy(
                "Maximum",
                data =>
                {
                    int max = int.MinValue;
                    foreach (int num in data)
                        if (num > max) max = num;
                    return max;
                }
            );
            processor.Process(new List<int> { 10, 20, 30, 40 });

            // Example 5: Text Transformation with Func<string, string>
            Console.WriteLine("--- Example 5: Text Transformation with Func<string, string> ---");
            var transformer = new TextTransformer(
                "Uppercase",
                text => text.ToUpper()
            );
            transformer.Transform("hello world");

            transformer.SetTransformStrategy(
                "Reverse",
                text =>
                {
                    char[] chars = text.ToCharArray();
                    Array.Reverse(chars);
                    return new string(chars);
                }
            );
            transformer.Transform("hello world");

            transformer.SetTransformStrategy(
                "Title Case",
                text =>
                {
                    System.Globalization.TextInfo textInfo = 
                        System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                    return textInfo.ToTitleCase(text);
                }
            );
            transformer.Transform("hello world");

            transformer.SetTransformStrategy(
                "Remove Vowels",
                text => System.Text.RegularExpressions.Regex.Replace(text, "[aeiouAEIOU]", "")
            );
            transformer.Transform("hello world");
        }
    }
}
