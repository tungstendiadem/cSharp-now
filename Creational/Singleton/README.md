# Singleton Pattern

The Singleton pattern is a creational pattern that restricts the instantiation of a class to a single object and provides a global point of access to it.

## Problem

Some classes should only have one instance:
- Database connections
- Configuration managers
- Logging services
- Thread pools
- Caches

Without the Singleton pattern, you might create multiple instances accidentally, leading to:
- Resource waste
- Inconsistent state
- Difficult debugging

## Classic Implementation

See `Classic/Singleton.cs` for the traditional Gang of Four implementation.

## Modern C# Alternatives

See `Modern/` directory for contemporary approaches:
- `SingletonWithStaticConstructor.cs` - Thread-safe static initialization
- `SingletonWithLazyT.cs` - Using `Lazy<T>` for deferred initialization
- `SingletonWithDependencyInjection.cs` - Service container approach

## Comparison

See `Comparison.md` for detailed trade-offs and recommendations.

## When to Use

- **Classic Singleton**: Legacy code or when you need explicit control
- **Static Constructor**: Simple, thread-safe, eager initialization
- **Lazy<T>**: Need lazy initialization with thread safety
- **Dependency Injection**: Modern applications, testability, flexibility

## Key Takeaway

Modern C# often eliminates the need for manual Singleton implementation. Dependency injection containers handle the singleton lifecycle automatically, making your code more testable and maintainable.
