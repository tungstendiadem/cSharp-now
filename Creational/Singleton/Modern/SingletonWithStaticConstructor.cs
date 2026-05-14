using System;

namespace CSharpNow.Creational.Singleton.Modern
{
    /// <summary>
    /// Modern Singleton Pattern - Using Static Constructor
    /// Thread-safe by design due to .NET's guarantee about static constructors.
    /// Simple and elegant.
    /// </summary>
    public class SingletonWithStaticConstructor
    {
        // Static instance initialized in static constructor
        private static readonly SingletonWithStaticConstructor _instance = new SingletonWithStaticConstructor();

        // Private constructor prevents instantiation
        private SingletonWithStaticConstructor()
        {
        }

        public static SingletonWithStaticConstructor Instance => _instance;

        public void DoSomething()
        {
            Console.WriteLine($"Static Constructor Singleton instance: {this.GetHashCode()}");
        }
    }

    /// <summary>
    /// Usage example
    /// </summary>
    public class StaticConstructorExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Singleton with Static Constructor ===");

            var instance1 = SingletonWithStaticConstructor.Instance;
            var instance2 = SingletonWithStaticConstructor.Instance;
            var instance3 = SingletonWithStaticConstructor.Instance;

            instance1.DoSomething();
            instance2.DoSomething();
            instance3.DoSomething();

            Console.WriteLine($"\nAll instances are the same: {instance1.GetHashCode() == instance2.GetHashCode() && instance2.GetHashCode() == instance3.GetHashCode()}");
        }
    }
}
