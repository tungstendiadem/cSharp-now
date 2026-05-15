using System;
using System.Collections.Generic;

namespace AdapterPattern.Modern;

/// <summary>
/// Modern C# Adapter Pattern Examples: Using C# 8+ Features
/// 
/// Demonstrates how modern C# language features can simplify adapter patterns:
/// - Extension Methods (zero-allocation adapters)
/// - Interface Default Implementations (adding behavior without wrapper classes)
/// - Delegates & Functional Composition (composable adapters)
/// - Generic Adapters with Constraints (reusable, type-safe adapters)
/// - Records & Pattern Matching (cleaner data transformation)
/// </summary>

// ===== SCENARIO =====
// Same legacy payment processor, but adapted using modern C# techniques

public class LegacyPaymentProcessor
{
    public decimal ProcessPayment(decimal amount, string accountNumber)
    {
        decimal fee = amount * 0.02m;
        decimal totalCharge = amount + fee;
        
        Console.WriteLine($"[Legacy] Processing ${amount} from account {accountNumber}");
        Console.WriteLine($"[Legacy] Charging fee: ${fee:F2}, Total: ${totalCharge:F2}");
        
        return totalCharge;
    }
}

// Modern data structures using records
public record PaymentRequest(string CardToken, decimal Amount, string Description);
public record PaymentResult(bool Success, decimal TotalAmount, string TransactionId, string? ErrorMessage = null);
public record DecodedCard(string AccountNumber, string CardholderName);

public interface IModernPaymentGateway
{
    PaymentResult ChargeCard(PaymentRequest request);
}

// ===== APPROACH 1: EXTENSION METHODS (Simplest & Most Idiomatic) =====
/// <summary>
/// Extension methods allow us to add adapter functionality without creating wrapper classes.
/// This is often the best choice for simple adaptations.
/// Zero allocation overhead - no intermediate objects created.
/// </summary>
public static class PaymentExtensions
{
    private static int _transactionCounter = 1000;
    private static readonly object _lock = new();

    /// <summary>
    /// Adapt LegacyPaymentProcessor to work with modern interfaces.
    /// Zero wrapper class overhead.
    /// </summary>
    public static PaymentResult ChargeCardLegacy(
        this LegacyPaymentProcessor processor, 
        PaymentRequest request)
    {
        try
        {
            string accountNumber = ExtractAccountNumber(request.CardToken);
            decimal totalCharge = processor.ProcessPayment(request.Amount, accountNumber);
            
            return new PaymentResult(
                Success: true,
                TotalAmount: totalCharge,
                TransactionId: GenerateTransactionId()
            );
        }
        catch (Exception ex)
        {
            return new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: ex.Message
            );
        }
    }

    /// <summary>
    /// Chain multiple adaptations together using extension method composition.
    /// </summary>
    public static PaymentResult WithLogging(this PaymentResult result)
    {
        Console.WriteLine($"[Extension] Transaction {result.TransactionId}: {(result.Success ? "Success" : "Failed")}");
        return result;
    }

    /// <summary>
    /// Apply a discount to the payment (adapter enhancement).
    /// </summary>
    public static PaymentResult WithDiscount(this PaymentResult result, decimal discountPercent)
    {
        if (result.Success)
        {
            decimal discount = result.TotalAmount * (discountPercent / 100m);
            decimal finalAmount = result.TotalAmount - discount;
            Console.WriteLine($"[Extension] Applied {discountPercent}% discount: ${discount:F2}, Final: ${finalAmount:F2}");
            
            return result with { TotalAmount = finalAmount };
        }
        return result;
    }

    private static string ExtractAccountNumber(string cardToken)
        => $"ACC-{cardToken[..4].ToUpper()}";

    private static string GenerateTransactionId()
    {
        lock (_lock)
        {
            return $"EXT-{++_transactionCounter}";
        }
    }
}

// ===== APPROACH 2: INTERFACE DEFAULT IMPLEMENTATIONS =====
/// <summary>
/// Interface defaults (C# 8+) allow us to add adapter behavior directly to interfaces.
/// Great when you control the interface definition.
/// </summary>
public interface IPaymentGatewayWithDefaults
{
    PaymentResult ChargeCard(PaymentRequest request);
    
    // Default implementation provides adapter logic
    PaymentResult ChargeCardWithRetry(PaymentRequest request, int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Console.WriteLine($"[Interface Default] Attempt {attempt}...");
                return ChargeCard(request);
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                Console.WriteLine($"[Interface Default] Failed: {ex.Message}, retrying...");
            }
        }
        
        return new PaymentResult(
            Success: false,
            TotalAmount: 0,
            TransactionId: "",
            ErrorMessage: "Max retries exceeded"
        );
    }
}

public class LegacyAdapterWithDefaults : IPaymentGatewayWithDefaults
{
    private readonly LegacyPaymentProcessor _processor;

    public LegacyAdapterWithDefaults(LegacyPaymentProcessor processor)
    {
        _processor = processor;
    }

    public PaymentResult ChargeCard(PaymentRequest request)
    {
        try
        {
            string accountNumber = $"ACC-{request.CardToken[..4].ToUpper()}";
            decimal totalCharge = _processor.ProcessPayment(request.Amount, accountNumber);
            
            return new PaymentResult(
                Success: true,
                TotalAmount: totalCharge,
                TransactionId: $"DEF-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            );
        }
        catch (Exception ex)
        {
            return new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: ex.Message
            );
        }
    }
}

// ===== APPROACH 3: FUNCTIONAL COMPOSITION (Delegates) =====
/// <summary>
/// Use delegates to create composable, chainable adapters.
/// Functional approach to adaptation.
/// </summary>
public class FunctionalAdapter
{
    private readonly LegacyPaymentProcessor _processor;
    
    // Type aliases for readability
    public delegate PaymentResult PaymentTransformer(PaymentRequest request);
    public delegate string TokenDecoder(string token);

    public FunctionalAdapter(LegacyPaymentProcessor processor)
    {
        _processor = processor;
    }

    /// <summary>
    /// Create a payment adapter using a custom token decoder function.
    /// </summary>
    public PaymentTransformer CreateAdapter(TokenDecoder decoder)
    {
        return (PaymentRequest request) =>
        {
            try
            {
                string accountNumber = decoder(request.CardToken);
                decimal totalCharge = _processor.ProcessPayment(request.Amount, accountNumber);
                
                return new PaymentResult(
                    Success: true,
                    TotalAmount: totalCharge,
                    TransactionId: $"FUNC-{Guid.NewGuid().ToString()[..8].ToUpper()}"
                );
            }
            catch (Exception ex)
            {
                return new PaymentResult(
                    Success: false,
                    TotalAmount: 0,
                    TransactionId: "",
                    ErrorMessage: ex.Message
                );
            }
        };
    }

    /// <summary>
    /// Compose adapters - apply multiple transformations in sequence.
    /// </summary>
    public static PaymentTransformer Compose(
        PaymentTransformer first,
        Func<PaymentResult, PaymentResult> second)
    {
        return (request) => second(first(request));
    }

    /// <summary>
    /// Pipe multiple result transformations.
    /// </summary>
    public static PaymentResult Pipe(
        PaymentResult result,
        params Func<PaymentResult, PaymentResult>[] transformations)
    {
        foreach (var transform in transformations)
        {
            result = transform(result);
        }
        return result;
    }
}

// ===== APPROACH 4: GENERIC ADAPTERS WITH CONSTRAINTS =====
/// <summary>
/// Generic adapter pattern reduces code duplication.
/// Type-safe and reusable across similar scenarios.
/// </summary>
public interface IAdapter<TSource, TTarget>
{
    TTarget Adapt(TSource source);
}

public class GenericPaymentAdapter : IAdapter<PaymentRequest, PaymentResult>
{
    private readonly LegacyPaymentProcessor _processor;
    private readonly Func<string, string> _tokenDecoder;

    public GenericPaymentAdapter(
        LegacyPaymentProcessor processor,
        Func<string, string> tokenDecoder)
    {
        _processor = processor;
        _tokenDecoder = tokenDecoder;
    }

    public PaymentResult Adapt(PaymentRequest source)
    {
        try
        {
            string accountNumber = _tokenDecoder(source.CardToken);
            decimal totalCharge = _processor.ProcessPayment(source.Amount, accountNumber);
            
            return new PaymentResult(
                Success: true,
                TotalAmount: totalCharge,
                TransactionId: $"GEN-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            );
        }
        catch (Exception ex)
        {
            return new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: ex.Message
            );
        }
    }
}

// ===== APPROACH 5: PATTERN MATCHING & RECORDS FOR TRANSFORMATION =====
/// <summary>
/// Use pattern matching to create intelligent adapters that handle different scenarios.
/// </summary>
public class SmartPaymentAdapter : IModernPaymentGateway
{
    private readonly LegacyPaymentProcessor _processor;

    public SmartPaymentAdapter(LegacyPaymentProcessor processor)
    {
        _processor = processor;
    }

    public PaymentResult ChargeCard(PaymentRequest request)
    {
        // Use pattern matching to validate and adapt
        return request switch
        {
            // Match specific conditions using when guards
            { Amount: <= 0 } => new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: "Invalid amount"
            ),
            
            { CardToken.Length: < 13 } => new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: "Invalid card token"
            ),
            
            // Process valid payment
            { Amount: > 0, CardToken.Length: >= 13 } => ProcessPaymentSafely(request),
            
            // Fallback
            _ => new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: "Unknown error"
            )
        };
    }

    private PaymentResult ProcessPaymentSafely(PaymentRequest request)
    {
        try
        {
            string accountNumber = DecodeToken(request.CardToken);
            decimal totalCharge = _processor.ProcessPayment(request.Amount, accountNumber);
            
            return new PaymentResult(
                Success: true,
                TotalAmount: totalCharge,
                TransactionId: $"SMART-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            );
        }
        catch (Exception ex)
        {
            return new PaymentResult(
                Success: false,
                TotalAmount: 0,
                TransactionId: "",
                ErrorMessage: ex.Message
            );
        }
    }

    private static string DecodeToken(string token) => $"ACC-{token[..4].ToUpper()}";
}

// ===== USAGE EXAMPLE =====
public class ModernAdapterExample
{
    public static void Run()
    {
        Console.WriteLine("=== Modern C# Adapter Pattern Examples ===\n");

        var paymentRequest = new PaymentRequest(
            CardToken: "4532123456789012",
            Amount: 99.99m,
            Description: "Product Purchase"
        );

        var processor = new LegacyPaymentProcessor();

        // Approach 1: Extension Methods
        Console.WriteLine("--- Approach 1: Extension Methods ---");
        var result1 = processor.ChargeCardLegacy(paymentRequest)
            .WithLogging()
            .WithDiscount(5);
        PrintResult(result1);

        // Approach 2: Interface Default Implementations
        Console.WriteLine("\n--- Approach 2: Interface Default Implementations ---");
        IPaymentGatewayWithDefaults adapter2 = new LegacyAdapterWithDefaults(processor);
        var result2 = adapter2.ChargeCard(paymentRequest);
        PrintResult(result2);

        // Approach 3: Functional Composition
        Console.WriteLine("\n--- Approach 3: Functional Composition ---");
        var functionalAdapter = new FunctionalAdapter(processor);
        var tokenDecoder = (string token) => $"ACC-{token[..4].ToUpper()}";
        var paymentTransformer = functionalAdapter.CreateAdapter(tokenDecoder);
        var result3 = paymentTransformer(paymentRequest);
        PrintResult(result3);

        // Approach 4: Generic Adapters
        Console.WriteLine("\n--- Approach 4: Generic Adapters ---");
        IAdapter<PaymentRequest, PaymentResult> adapter4 = 
            new GenericPaymentAdapter(processor, tokenDecoder);
        var result4 = adapter4.Adapt(paymentRequest);
        PrintResult(result4);

        // Approach 5: Pattern Matching & Smart Adapter
        Console.WriteLine("\n--- Approach 5: Smart Pattern Matching Adapter ---");
        IModernPaymentGateway adapter5 = new SmartPaymentAdapter(processor);
        var result5 = adapter5.ChargeCard(paymentRequest);
        PrintResult(result5);

        // Test invalid payment with pattern matching
        var invalidRequest = new PaymentRequest(
            CardToken: "123",  // Too short
            Amount: 99.99m,
            Description: "Invalid Payment"
        );
        Console.WriteLine("\nTesting invalid request with Smart Adapter:");
        var invalidResult = adapter5.ChargeCard(invalidRequest);
        PrintResult(invalidResult);
    }

    private static void PrintResult(PaymentResult result)
    {
        Console.WriteLine($"  Success: {result.Success}");
        Console.WriteLine($"  Total Amount: ${result.TotalAmount:F2}");
        Console.WriteLine($"  Transaction ID: {result.TransactionId}");
        if (!string.IsNullOrEmpty(result.ErrorMessage))
            Console.WriteLine($"  Error: {result.ErrorMessage}");
    }
}
