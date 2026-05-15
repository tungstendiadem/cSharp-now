# Adapter Pattern

## What is the Adapter Pattern?

The **Adapter** pattern converts the interface of a class into another interface that clients expect. It allows classes with incompatible interfaces to work together by wrapping an existing class with a new interface.

### Problem It Solves
- You want to use a class, but its interface doesn't match what your code expects
- You have legacy code that needs to work with new code
- You need to integrate third-party libraries with incompatible interfaces
- You want to create a unified interface for multiple implementations

### Real-World Analogy
Like a power adapter that converts a US plug to a European socket—the electricity is the same, but the interface is transformed.

---

## Classic Approach

### Structure
```
Client → Target Interface ← Adapter ← Adaptee (Incompatible Class)
```

**Key Components:**
- **Target Interface**: The interface clients expect
- **Adapter**: Converts the Adaptee to the Target interface
- **Adaptee**: The existing class with an incompatible interface
- **Client**: Uses the Target interface

**Characteristics:**
- Explicit interface implementation
- Class adapter (inheritance) or object adapter (composition)
- Boilerplate-heavy wrapper classes

---

## Modern C# Approach

Modern C# offers several alternatives:

### 1. **Extension Methods** (Simplest)
- Add methods to existing classes without creating wrapper classes
- No need for separate adapter objects
- Cleaner, more direct

### 2. **Interface Implementation** (Flexible)
- Use interface defaults to add behavior
- Combine with records and pattern matching
- More maintainable than traditional adapters

### 3. **Delegates & Functional Composition** (Functional)
- Use `Func<T, TResult>` for simple conversions
- Compose adapters functionally
- Minimal overhead

### 4. **Generic Adapters with Constraints**
- Leverage generics and constraints for type safety
- Reduce code duplication

---

## Comparison

| Aspect | Classic Approach | Modern C# |
|--------|------------------|-----------|
| **Boilerplate** | High (wrapper class required) | Low (extension methods, defaults) |
| **Performance** | Allocation overhead | Often zero-cost abstractions |
| **Discoverability** | Limited (explicit wrappers) | Better (extensions on IntelliSense) |
| **Flexibility** | Good (composition) | Excellent (multiple strategies) |
| **Readability** | Verbose | Concise |

---

## When to Use Each

### Classic Adapter
- When you need complex transformation logic
- When adapting multiple incompatible interfaces
- When you need formal separation of concerns
- In large enterprise systems with strict architecture

### Modern C# (Extension Methods)
- For simple conversions or additions
- When adapting a single interface
- When performance matters (zero allocations)
- In codebases where extension methods are already used

### Modern C# (Interface Defaults)
- When you control the interface definition
- When building libraries with multiple implementations
- When you want to add behavior without breaking changes

### Functional Composition
- For chainable, composable adapters
- In functional-style codebases
- For quick, temporary adapters

---

## Examples Included

- **ClassicAdapter.cs** - Traditional class adapter pattern with inheritance and composition
- **ModernAdapter.cs** - Modern C# approaches using extension methods, interface defaults, and functional composition

---

## Key Takeaways

1. **Extension methods** are often the best choice for simple interface adaptation
2. **Interface defaults** provide elegant solutions when you control the interface
3. **Generics** reduce code duplication for similar adapters
4. **Delegates** enable functional composition of adapters
5. Modern C# reduces the need for explicit adapter classes in many scenarios
