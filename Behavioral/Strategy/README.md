# Strategy Pattern

## Overview

The Strategy Pattern is a behavioral design pattern that defines a family of algorithms, encapsulates each one, and makes them interchangeable. It lets the algorithm vary independently from clients that use it.

## Problem It Solves

Without the Strategy Pattern, you might end up with:
- Large conditional statements (if/switch) choosing between algorithm implementations
- Tight coupling between the client and specific algorithm implementations
- Difficulty adding new algorithms without modifying existing code
- Code duplication across different contexts

## Strategy Pattern Implementation

The classic Strategy pattern involves:
1. **Strategy Interface** - Defines the contract for algorithm implementations
2. **Concrete Strategies** - Implement different algorithms
3. **Context** - Uses a strategy to perform operations
4. **Client** - Selects and configures the appropriate strategy

### Example: Payment Processing

```csharp
// Strategy Interface
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

// Concrete Strategies
public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of ${amount}");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of ${amount}");
    }
}

// Context
public class PaymentProcessor
{
    private IPaymentStrategy _strategy;

    public PaymentProcessor(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ProcessPayment(decimal amount)
    {
        _strategy.Pay(amount);
    }
}

// Usage
var processor = new PaymentProcessor(new CreditCardPayment());
processor.ProcessPayment(100);
```

## C# Native Alternatives

### 1. **Delegates**

C# delegates provide a lightweight alternative to the Strategy pattern for simple algorithm selection.

**Advantages:**
- Minimal boilerplate code
- No separate interface or class definitions needed
- Good for simple, single-method strategies

**Disadvantages:**
- Less type-safe than interfaces
- Harder to implement complex logic
- Harder to maintain state

```csharp
// Define a delegate type
public delegate void PaymentStrategy(decimal amount);

// Define strategies as methods
public class PaymentMethods
{
    public static void CreditCardPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of ${amount}");
    }

    public static void PayPalPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of ${amount}");
    }
}

// Client
public class PaymentProcessor
{
    private PaymentStrategy _strategy;

    public PaymentProcessor(PaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ProcessPayment(decimal amount)
    {
        _strategy(amount);
    }
}

// Usage
var processor = new PaymentProcessor(PaymentMethods.CreditCardPayment);
processor.ProcessPayment(100);
```

### 2. **Func<T, TResult> and Action<T>**

Generic delegates that eliminate the need for custom delegate declarations.

**Advantages:**
- Built-in types, no custom declarations
- Maximum simplicity
- Works well with lambda expressions

**Disadvantages:**
- Less descriptive naming
- No domain-specific semantics
- Harder to discover available strategies

```csharp
// Using Action for void return
public class PaymentProcessor
{
    private Action<decimal> _strategy;

    public PaymentProcessor(Action<decimal> strategy)
    {
        _strategy = strategy;
    }

    public void ProcessPayment(decimal amount)
    {
        _strategy(amount);
    }
}

// Usage with lambda
var processor = new PaymentProcessor(amount => 
    Console.WriteLine($"Processing credit card payment of ${amount}"));
processor.ProcessPayment(100);
```

### 3. **Strategy Pattern (Interface-Based)**

The traditional approach with dedicated interfaces.

**Advantages:**
- Most explicit and discoverable
- Best for complex strategies with multiple methods
- Type-safe and self-documenting
- Easy to add shared behavior through inheritance

**Disadvantages:**
- More boilerplate code
- Overkill for simple operations

```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
    string GetPaymentMethod();
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of ${amount}");
    }

    public string GetPaymentMethod() => "Credit Card";
}
```

### 4. **Func<T, TResult> with Return Values**

For strategies that need to return results.

```csharp
// Strategy that calculates and returns a value
public class DiscountCalculator
{
    private Func<decimal, decimal> _discountStrategy;

    public DiscountCalculator(Func<decimal, decimal> discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal CalculatePrice(decimal originalPrice)
    {
        return _discountStrategy(originalPrice);
    }
}

// Usage
var calculator = new DiscountCalculator(price => price * 0.9m); // 10% discount
decimal finalPrice = calculator.CalculatePrice(100);
```

## Comparison Table

| Feature | Strategy Pattern | Delegates | Func/Action | 
|---------|------------------|-----------|------------|
| Complexity | Medium | Low | Very Low |
| Boilerplate | High | Medium | Low |
| Type Safety | High | Medium | High |
| Discoverability | High | Medium | Low |
| Multiple Methods | Excellent | Good | Poor |
| State Management | Excellent | Good | Fair |
| Lambda Support | Good | Good | Excellent |
| Domain Semantics | Excellent | Good | Poor |

## When to Use Each Approach

### Use Strategy Pattern (Interface) When:
- You have complex algorithms with multiple methods
- You need to share state across the strategy
- You want self-documenting code
- You have many different implementations
- You need to extend strategies with inheritance

### Use Delegates When:
- You have simple, single-method algorithms
- You want less boilerplate than interfaces
- You're combining with inheritance hierarchies
- You need to follow convention in your codebase

### Use Func/Action When:
- You have very simple, one-liner strategies
- You're working with LINQ or functional programming patterns
- You want maximum conciseness
- You're already using lambda expressions throughout your code

## Example: Real-World Sorting Strategy

```csharp
// Strategy Pattern Approach
public interface ISortStrategy
{
    void Sort(int[] data);
}

public class BubbleSort : ISortStrategy
{
    public void Sort(int[] data) { /* ... */ }
}

// Func Approach
public class Sorter
{
    public void Sort(int[] data, Action<int[]> sortStrategy)
    {
        sortStrategy(data);
    }
}

// Usage
var sorter = new Sorter();
sorter.Sort(data, arr => Array.Sort(arr));
```

## Key Takeaways

1. **C# provides multiple mechanisms** for implementing strategy-like behavior
2. **Choose based on complexity**: Simple = Func/Action, Complex = Interface
3. **Delegates and generics reduce boilerplate** compared to traditional patterns
4. **Modern C# encourages functional approaches** through Func/Action/LINQ
5. **The intent remains the same**: Encapsulate interchangeable algorithms
