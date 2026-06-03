// Strategy Pattern: Interface-Based Approach
// Best for: Complex algorithms with multiple methods, state management, self-documenting code

using System;

namespace StrategyPatternExamples
{
    // Strategy Interface - Defines the contract for payment strategies
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
        string GetPaymentMethod();
        bool ValidatePayment();
    }

    // Concrete Strategy 1: Credit Card Payment
    public class CreditCardPayment : IPaymentStrategy
    {
        private readonly string _cardNumber;
        private readonly string _cardHolder;

        public CreditCardPayment(string cardNumber, string cardHolder)
        {
            _cardNumber = cardNumber;
            _cardHolder = cardHolder;
        }

        public void Pay(decimal amount)
        {
            if (ValidatePayment())
            {
                Console.WriteLine($"Processing credit card payment of ${amount}");
                Console.WriteLine($"Card: {MaskCardNumber(_cardNumber)}");
                Console.WriteLine($"Holder: {_cardHolder}");
                Console.WriteLine("✓ Payment processed successfully");
            }
            else
            {
                Console.WriteLine("✗ Payment validation failed");
            }
        }

        public string GetPaymentMethod() => "Credit Card";

        public bool ValidatePayment()
        {
            // Card validation logic
            return _cardNumber.Length == 16 && !string.IsNullOrEmpty(_cardHolder);
        }

        private string MaskCardNumber(string cardNumber)
        {
            return "****-****-****-" + cardNumber.Substring(cardNumber.Length - 4);
        }
    }

    // Concrete Strategy 2: PayPal Payment
    public class PayPalPayment : IPaymentStrategy
    {
        private readonly string _email;

        public PayPalPayment(string email)
        {
            _email = email;
        }

        public void Pay(decimal amount)
        {
            if (ValidatePayment())
            {
                Console.WriteLine($"Processing PayPal payment of ${amount}");
                Console.WriteLine($"Account: {_email}");
                Console.WriteLine("✓ Payment processed successfully");
            }
            else
            {
                Console.WriteLine("✗ Payment validation failed");
            }
        }

        public string GetPaymentMethod() => "PayPal";

        public bool ValidatePayment()
        {
            // Email validation logic
            return _email.Contains("@") && _email.Contains(".");
        }
    }

    // Concrete Strategy 3: Bank Transfer
    public class BankTransferPayment : IPaymentStrategy
    {
        private readonly string _accountNumber;
        private readonly string _routingNumber;

        public BankTransferPayment(string accountNumber, string routingNumber)
        {
            _accountNumber = accountNumber;
            _routingNumber = routingNumber;
        }

        public void Pay(decimal amount)
        {
            if (ValidatePayment())
            {
                Console.WriteLine($"Processing bank transfer of ${amount}");
                Console.WriteLine($"Account: {MaskAccountNumber(_accountNumber)}");
                Console.WriteLine("✓ Payment processed successfully");
            }
            else
            {
                Console.WriteLine("✗ Payment validation failed");
            }
        }

        public string GetPaymentMethod() => "Bank Transfer";

        public bool ValidatePayment()
        {
            return _accountNumber.Length > 0 && _routingNumber.Length == 9;
        }

        private string MaskAccountNumber(string accountNumber)
        {
            return "****" + accountNumber.Substring(accountNumber.Length - 4);
        }
    }

    // Context - Uses a payment strategy to process payments
    public class PaymentProcessor
    {
        private IPaymentStrategy _strategy;

        public PaymentProcessor(IPaymentStrategy strategy)
        {
            _strategy = strategy;
        }

        // Allows changing strategy at runtime
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _strategy = strategy;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"\n--- Processing payment via {_strategy.GetPaymentMethod()} ---");
            _strategy.Pay(amount);
        }
    }

    // Demo
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Strategy Pattern: Interface-Based Approach ===\n");

            // Create payment processor with credit card strategy
            var processor = new PaymentProcessor(
                new CreditCardPayment("1234567890123456", "John Doe")
            );

            processor.ProcessPayment(99.99m);

            // Switch to PayPal strategy at runtime
            processor.SetPaymentStrategy(new PayPalPayment("john@example.com"));
            processor.ProcessPayment(49.99m);

            // Switch to Bank Transfer strategy
            processor.SetPaymentStrategy(
                new BankTransferPayment("123456789", "987654321")
            );
            processor.ProcessPayment(299.99m);

            // Invalid payment example
            Console.WriteLine();
            processor.SetPaymentStrategy(new PayPalPayment("invalid-email"));
            processor.ProcessPayment(50m);
        }
    }
}
