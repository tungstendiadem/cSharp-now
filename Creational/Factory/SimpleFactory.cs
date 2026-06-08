using System;

namespace FactoryPattern.Classic
{
    // Abstract base class
    public abstract class Animal
    {
        public abstract void Speak();
        public abstract string GetType();
    }

    // Concrete implementations
    public class Dog : Animal
    {
        public override void Speak() => Console.WriteLine("Woof!");
        public override string GetType() => "Dog";
    }

    public class Cat : Animal
    {
        public override void Speak() => Console.WriteLine("Meow!");
        public override string GetType() => "Cat";
    }

    public class Bird : Animal
    {
        public override void Speak() => Console.WriteLine("Tweet!");
        public override string GetType() => "Bird";
    }

    // Simple Factory
    public class AnimalFactory
    {
        public static Animal CreateAnimal(string type)
        {
            return type.ToLower() switch
            {
                "dog" => new Dog(),
                "cat" => new Cat(),
                "bird" => new Bird(),
                _ => throw new ArgumentException($"Unknown animal type: {type}")
            };
        }
    }

    // Usage
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Simple Factory Pattern ===\n");

            var dog = AnimalFactory.CreateAnimal("dog");
            Console.WriteLine($"Created: {dog.GetType()}");
            dog.Speak();

            var cat = AnimalFactory.CreateAnimal("cat");
            Console.WriteLine($"\nCreated: {cat.GetType()}");
            cat.Speak();

            var bird = AnimalFactory.CreateAnimal("bird");
            Console.WriteLine($"\nCreated: {bird.GetType()}");
            bird.Speak();
        }
    }
}
