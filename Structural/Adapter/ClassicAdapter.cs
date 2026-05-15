using System;

namespace AdapterPattern.Classic;

/// <summary>
/// Classic Adapter Pattern Example: Class Adapter (Inheritance-based)
/// 
/// Scenario: We have a legacy PaymentProcessor that works with a specific interface,
/// but we need to integrate it with a new system that expects a different interface.
/// </summary>

// ===== ADAPTEE =====
// This is the existing class with an incompatible interface
public class LegacyPaymentProcessor
{
    public decimal ProcessPayment(decimal amount, string accountNumber)
    {
        // Simulate processing
        decimal fee = amount * 0.02m;
        decimal totalCharge = amount + fee;
        
        Console.WriteLine($"[Legacy] Processing ${amount} from account {accountNumber}");
        Console.WriteLine($"[Legacy] Charging fee: ${fee:F2}, Total: ${totalCharge:F2}");
        
        return totalCharge;
    }
}

// ===== TARGET INTERFACE =====
// This is what the new system expects
public interface IModernPaymentGateway
{
    PaymentResult ChargeCard(PaymentRequest request);
}

public class PaymentRequest
{
    public required string CardToken { get; set; }
    public required decimal Amount { get; set; }
    public required string Description { get; set; }
}

public class PaymentResult
{
    public required bool Success { get; set; }
    public required decimal TotalAmount { get; set; }
    public required string TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
}

// ===== CLASS ADAPTER (using composition - Object Adapter) =====
// This is more practical than inheritance-based adapters
public class ClassAdapter : IModernPaymentGateway
{
    private readonly LegacyPaymentProcessor _legacyProcessor;
    private int _transactionCounter = 1000;

    public ClassAdapter(LegacyPaymentProcessor legacyProcessor)
    {
        _legacyProcessor = legacyProcessor;
    }

    public PaymentResult ChargeCard(PaymentRequest request)
    {
        try
        {
            // Extract account number from token (adaptation logic)
            string accountNumber = ExtractAccountNumber(request.CardToken);
            
            // Call the legacy interface with adapted parameters
            decimal totalCharge = _legacyProcessor.ProcessPayment(request.Amount, accountNumber);
            
            return new PaymentResult
            {
                Success = true,
                TotalAmount = totalCharge,
                TransactionId = GenerateTransactionId()
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                Success = false,
                TotalAmount = 0,
                TransactionId = "",
                ErrorMessage = ex.Message
            };
        }
    }

    private string ExtractAccountNumber(string cardToken)
    {
        // Simulate extracting account number from token
        return $"ACC-{cardToken.Substring(0, Math.Min(4, cardToken.Length)).ToUpper()}";
    }

    private string GenerateTransactionId()
    {
        return $"TXN-{++_transactionCounter}";
    }
}

// ===== OBJECT ADAPTER (Composition-based) =====
// More flexible than class adapter - can adapt multiple adaptees
public class ObjectAdapter : IModernPaymentGateway
{
    private readonly LegacyPaymentProcessor _processor;
    private readonly ICardTokenDecoder _tokenDecoder;

    public ObjectAdapter(LegacyPaymentProcessor processor, ICardTokenDecoder tokenDecoder)
    {
        _processor = processor;
        _tokenDecoder = tokenDecoder;
    }

    public PaymentResult ChargeCard(PaymentRequest request)
    {
        try
        {
            var decodedCard = _tokenDecoder.Decode(request.CardToken);
            decimal totalCharge = _processor.ProcessPayment(request.Amount, decodedCard.AccountNumber);
            
            return new PaymentResult
            {
                Success = true,
                TotalAmount = totalCharge,
                TransactionId = $"OBJ-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };
        }
        catch (Exception ex)
        {
            return new PaymentResult
            {
                Success = false,
                TotalAmount = 0,
                TransactionId = "",
                ErrorMessage = ex.Message
            };
        }
    }
}

public interface ICardTokenDecoder
{
    DecodedCard Decode(string token);
}

public class DecodedCard
{
    public required string AccountNumber { get; set; }
    public required string CardholderName { get; set; }
}

public class StandardCardTokenDecoder : ICardTokenDecoder
{
    public DecodedCard Decode(string token)
    {
        // Simulate decoding a token
        return new DecodedCard
        {
            AccountNumber = $"ACC-{token[..4].ToUpper()}",
            CardholderName = "John Doe"
        };
    }
}

// ===== USAGE EXAMPLE =====
public class ClassicAdapterExample
{
    public static void Run()
    {
        Console.WriteLine("=== Classic Adapter Pattern ===\n");

        var paymentRequest = new PaymentRequest
        {
            CardToken = "4532123456789012",
            Amount = 99.99m,
            Description = "Product Purchase"
        };

        // Class Adapter Approach
        Console.WriteLine("--- Class Adapter Approach ---");
        var legacyProcessor = new LegacyPaymentProcessor();
        var classAdapter = new ClassAdapter(legacyProcessor);
        var result1 = classAdapter.ChargeCard(paymentRequest);
        PrintResult(result1);

        Console.WriteLine("\n--- Object Adapter Approach ---");
        var tokenDecoder = new StandardCardTokenDecoder();
        var objectAdapter = new ObjectAdapter(legacyProcessor, tokenDecoder);
        var result2 = objectAdapter.ChargeCard(paymentRequest);
        PrintResult(result2);
    }

    private static void PrintResult(PaymentResult result)
    {
        Console.WriteLine($"Success: {result.Success}");
        Console.WriteLine($"Total Amount: ${result.TotalAmount:F2}");
        Console.WriteLine($"Transaction ID: {result.TransactionId}");
        if (!string.IsNullOrEmpty(result.ErrorMessage))
            Console.WriteLine($"Error: {result.ErrorMessage}");
    }
}
