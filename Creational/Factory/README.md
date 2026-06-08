# Factory Pattern

## Overview

The Factory Pattern is a creational design pattern that provides an interface for creating objects without specifying their exact classes. Instead of using `new` to instantiate objects directly, you use a factory method or factory class to handle object creation.

## Classic Factory Pattern Implementation

### Simple Factory
```csharp
public class AnimalFactory
{
    public static Animal CreateAnimal(string type)
    {
        return type switch
        {
            "dog" => new Dog(),
            "cat" => new Cat(),
            "bird" => new Bird(),
            _ => throw new ArgumentException($"Unknown animal type: {type}")
        };
    }
}

public abstract class Animal
{
    public abstract void Speak();
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

public class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow!");
}

// Usage
var dog = AnimalFactory.CreateAnimal("dog");
dog.Speak();
```

### Factory Method Pattern
```csharp
public abstract class AnimalFactory
{
    public abstract Animal CreateAnimal();
}

public class DogFactory : AnimalFactory
{
    public override Animal CreateAnimal() => new Dog();
}

public class CatFactory : AnimalFactory
{
    public override Animal CreateAnimal() => new Cat();
}

// Usage
AnimalFactory factory = new DogFactory();
var animal = factory.CreateAnimal();
animal.Speak();
```

## Modern C# Alternatives

### 1. **Dependency Injection (DI) Container**
Modern approach using .NET's built-in DI:

```csharp
// Startup configuration
var builder = new ServiceCollection();
builder.AddScoped<Dog>();
builder.AddScoped<Cat>();
builder.AddScoped<Bird>();

// At runtime, resolve based on type name
var serviceProvider = builder.BuildServiceProvider();
var animal = serviceProvider.GetService(Type.GetType($"MyNamespace.{type}"));
```

**Advantages:**
- Decouples object creation from business logic
- Easier to test with mocks
- Supports lifetime management (singleton, scoped, transient)
- Integrates seamlessly with ASP.NET Core

### 2. **Reflection with Type Registry**
```csharp
public class AnimalRegistry
{
    private static readonly Dictionary<string, Type> Registry = new()
    {
        { "dog", typeof(Dog) },
        { "cat", typeof(Cat) },
        { "bird", typeof(Bird) }
    };

    public static Animal Create(string type)
    {
        if (Registry.TryGetValue(type, out var animalType))
        {
            return (Animal)Activator.CreateInstance(animalType)!;
        }
        throw new ArgumentException($"Unknown animal type: {type}");
    }
}
```

**Advantages:**
- No explicit factory code needed
- Easy to extend with new types
- Centralized type mapping

### 3. **Generic Factory with Constraints**
```csharp
public class GenericFactory<T> where T : class, new()
{
    public T Create() => new T();
}

// Usage
var dogFactory = new GenericFactory<Dog>();
var dog = dogFactory.Create();
```

**Advantages:**
- Type-safe at compile time
- Minimal boilerplate
- Useful for simple scenarios

### 4. **Factory with Func Delegates**
```csharp
public class DelegateFactory
{
    private readonly Dictionary<string, Func<Animal>> _factories = new()
    {
        { "dog", () => new Dog() },
        { "cat", () => new Cat() },
        { "bird", () => new Bird() }
    };

    public Animal Create(string type)
    {
        if (_factories.TryGetValue(type, out var factory))
        {
            return factory();
        }
        throw new ArgumentException($"Unknown type: {type}");
    }
}

// Usage
var delegateFactory = new DelegateFactory();
var animal = delegateFactory.Create("dog");
```

**Advantages:**
- Flexible and expressive
- Can capture context and parameters
- Easy to configure

### 5. **Abstract Factory Pattern (Modern)**
```csharp
public interface IPetFactory
{
    Animal CreateAnimal();
    string GetDescription();
}

public class DomesticPetFactory : IPetFactory
{
    public Animal CreateAnimal() => new Dog();
    public string GetDescription() => "Domestic Pet";
}

public class WildAnimalFactory : IPetFactory
{
    public Animal CreateAnimal() => new Lion();
    public string GetDescription() => "Wild Animal";
}

// With DI
var builder = new ServiceCollection();
builder.AddScoped<IPetFactory, DomesticPetFactory>();
```

**Advantages:**
- Creates families of related objects
- Easily swappable implementations
- Works well with dependency injection

## Comparison Table

| Pattern | Simplicity | Testability | Flexibility | Modern C# | Use Case |
|---------|-----------|------------|-----------|----------|----------|
| Simple Factory | ⭐⭐⭐ | ⭐⭐ | ⭐⭐ | ⭐ | Quick scripts, simple apps |
| Factory Method | ⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐ | Inheritance hierarchies |
| DI Container | ⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | Enterprise apps, ASP.NET Core |
| Reflection Registry | ⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ | Plugin systems, data-driven |
| Generic Factory | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐ | Type-safe creation |
| Func Delegates | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ | Configuration, flexible setup |

## Recommendations

- **ASP.NET Core Applications**: Use built-in DI container
- **Plugin/Extensible Systems**: Use reflection with type registry
- **Simple CRUD Operations**: Use generic factories or delegates
- **Complex Object Families**: Use abstract factory with DI
- **Legacy/Non-DI Applications**: Use simple or factory method pattern

## When to Use Factory Pattern

✅ When you need to decouple object creation from usage  
✅ When object creation logic is complex  
✅ When you support multiple implementations  
✅ When you want to change implementations without changing client code

❌ When DI container can handle it (prefer DI)  
❌ When creating simple objects without dependencies  
❌ When overcomplicating a simple problem
