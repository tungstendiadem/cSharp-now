// Strategy Pattern: Custom Delegates Approach
// Best for: Simple, single-method strategies with less boilerplate than interfaces

using System;

namespace StrategyPatternExamples
{
    // Define custom delegate types for different payment strategies
    public delegate void PaymentDelegate(decimal amount);
    public delegate bool ValidationDelegate();
    public delegate string DescriptionDelegate();

    // Payment method implementations using delegates
    public class PaymentMethods
    {
        // Credit Card Payment
        public static void ProcessCreditCardPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount}");
            Console.WriteLine("Card: ****-****-****-1234");
            Console.WriteLine("✓ Payment processed successfully");
        }

        // PayPal Payment
        public static void ProcessPayPalPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of ${amount}");
            Console.WriteLine("Account: user@example.com");
            Console.WriteLine("✓ Payment processed successfully");
        }

        // Bank Transfer Payment
        public static void ProcessBankTransferPayment(decimal amount)
        {
            Console.WriteLine($"Processing bank transfer of ${amount}");
            Console.WriteLine("Account: ****5678");
            Console.WriteLine("✓ Payment processed successfully");
        }

        // Cryptocurrency Payment
        public static void ProcessCryptoPayment(decimal amount)
        {
            Console.WriteLine($"Processing cryptocurrency payment of ${amount} BTC");
            Console.WriteLine("Wallet: 1A1z7agoat3dLSt....");
            Console.WriteLine("✓ Payment processed successfully");
        }
    }

    // Validation methods
    public class ValidationMethods
    {
        public static bool ValidateCreditCard()
        {
            Console.WriteLine("Validating credit card...");
            return true;
        }

        public static bool ValidatePayPal()
        {
            Console.WriteLine("Validating PayPal account...");
            return true;
        }

        public static bool ValidateBankAccount()
        {
            Console.WriteLine("Validating bank account...");
            return true;
        }
    }

    // Context - Uses delegates to process payments
    public class PaymentProcessorWithDelegates
    {
        private PaymentDelegate _paymentStrategy;
        private ValidationDelegate _validationStrategy;
        private DescriptionDelegate _descriptionStrategy;

        public PaymentProcessorWithDelegates(
            PaymentDelegate paymentStrategy,
            ValidationDelegate validationStrategy,
            DescriptionDelegate descriptionStrategy)
        {
            _paymentStrategy = paymentStrategy;
            _validationStrategy = validationStrategy;
            _descriptionStrategy = descriptionStrategy;
        }

        // Allow switching strategies at runtime
        public void SetPaymentStrategy(
            PaymentDelegate paymentStrategy,
            ValidationDelegate validationStrategy,
            DescriptionDelegate descriptionStrategy)
        {
            _paymentStrategy = paymentStrategy;
            _validationStrategy = validationStrategy;
            _descriptionStrategy = descriptionStrategy;
        }

        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"\n--- Processing payment via {_descriptionStrategy()} ---");
            
            if (_validationStrategy())
            {
                _paymentStrategy(amount);
            }
            else
            {
                Console.WriteLine("✗ Payment validation failed");
            }
        }
    }

    // Demo
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Strategy Pattern: Custom Delegates Approach ===\n");

            // Create processor with credit card strategy
            var processor = new PaymentProcessorWithDelegates(
                PaymentMethods.ProcessCreditCardPayment,
                ValidationMethods.ValidateCreditCard,
                () => "Credit Card"
            );

            processor.ProcessPayment(99.99m);

            // Switch to PayPal strategy
            processor.SetPaymentStrategy(
                PaymentMethods.ProcessPayPalPayment,
                ValidationMethods.ValidatePayPal,
                () => "PayPal"
            );
            processor.ProcessPayment(49.99m);

            // Switch to Bank Transfer strategy
            processor.SetPaymentStrategy(
                PaymentMethods.ProcessBankTransferPayment,
                ValidationMethods.ValidateBankAccount,
                () => "Bank Transfer"
            );
            processor.ProcessPayment(299.99m);

            // Switch to Crypto strategy
            processor.SetPaymentStrategy(
                PaymentMethods.ProcessCryptoPayment,
                () => { Console.WriteLine("Validating cryptocurrency wallet..."); return true; },
                () => "Cryptocurrency"
            );
            processor.ProcessPayment(0.005m);

            // Using lambda expressions with delegates
            Console.WriteLine("\n--- Using Lambda Expressions with Delegates ---");
            processor.SetPaymentStrategy(
                amount => Console.WriteLine($"Processing custom payment of ${amount}"),
                () => { Console.WriteLine("Custom validation"); return true; },
                () => "Custom Strategy"
            );
            processor.ProcessPayment(75m);
        }
    }
}
