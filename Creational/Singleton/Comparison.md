# Singleton Pattern: Comparison

## Overview

This document compares the classic Singleton pattern with modern C# alternatives.

## 1. Classic Singleton (Double-Checked Locking)

### Pros
- Explicit control over instantiation
- Works in any context
- Widely recognized pattern

### Cons
- Verbose and error-prone
- Double-checked locking complexity
- Hard to test (global state)
- Tightly coupled to client code
- Not friendly with dependency injection

### When to Use
- Legacy code
- Very simple scenarios where DI isn't available
- When you need explicit control

```csharp
var instance = ClassicSingleton.Instance; // Static access
```

---

## 2. Static Constructor Initialization

### Pros
- Thread-safe by design (CLR guarantees)
- Simple and concise
- Eager initialization ensures early error detection
- No locking overhead

### Cons
- Eager initialization (not lazy)
- Still uses global state
- Hard to test
- Not compatible with DI containers

### When to Use
- When you need simple, thread-safe singleton
- Initialization is not expensive
- Legacy code without DI

```csharp
private static readonly MyClass _instance = new MyClass();
public static MyClass Instance => _instance;
```

---

## 3. Lazy<T> Pattern

### Pros
- Lazy initialization (only when needed)
- Thread-safe by design
- Clean and simple
- Good compromise between classic and modern approaches

### Cons
- Still uses global state
- Hard to test
- Not ideal for DI-based applications

### When to Use
- Expensive initialization
- Uncertain if instance will be used
- Non-DI applications that need lazy initialization
- Legacy code modernization

```csharp
private static readonly Lazy<MyClass> _instance = 
    new Lazy<MyClass>(() => new MyClass());
public static MyClass Instance => _instance.Value;
```

---

## 4. Dependency Injection (Recommended)

### Pros
- No global state
- Easy to test (mock/inject dependencies)
- Flexible lifecycle management
- Framework handles thread safety
- Modern best practice
- Works seamlessly with ASP.NET Core, WPF, etc.

### Cons
- Requires DI container setup
- Not suitable for static contexts
- Need to learn DI patterns

### When to Use
- **Modern applications** (ASP.NET Core, WPF, Console apps with DI)
- **Testable code** is a requirement
- **Large applications** with many dependencies
- **Team standards** favor dependency injection

```csharp
services.AddSingleton<IMyService, MyService>();
// Injected automatically where needed
```

---

## Lifecycle Comparison

| Approach | Thread-Safe | Lazy Init | Testable | DI-Friendly | Complexity |
|----------|:-----------:|:---------:|:--------:|:-----------:|:-----------:|
| Classic | ✓ | ✗ | ✗ | ✗ | High |
| Static Constructor | ✓ | ✗ | ✗ | ✗ | Low |
| Lazy<T> | ✓ | ✓ | ✗ | ✗ | Low |
| Dependency Injection | ✓ | ✓ | ✓ | ✓ | Medium |

---

## Modern C# Recommendation

**Use Dependency Injection containers** (e.g., Microsoft.Extensions.DependencyInjection)

Reasons:
1. **Testability**: Inject mocks/stubs for unit testing
2. **Flexibility**: Change implementation without changing client code
3. **Framework Integration**: Works natively with modern .NET ecosystems
4. **Separation of Concerns**: No global state, no tight coupling
5. **Industry Standard**: Widely adopted in enterprise applications

### Example Migration Path

```csharp
// OLD: Classic Singleton
var config = AppConfiguration.Instance;

// NEW: Dependency Injection
public class MyService
{
    private readonly IConfiguration _config;
    
    public MyService(IConfiguration config)
    {
        _config = config; // Injected
    }
}

// Registration
services.AddSingleton<IConfiguration, AppConfiguration>();
```

---

## Key Takeaway

While the classic Singleton pattern is still valid, modern C# developers should prefer **dependency injection containers** for managing singleton lifecycles. This approach provides better testability, flexibility, and aligns with contemporary application architecture.
