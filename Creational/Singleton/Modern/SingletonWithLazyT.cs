using System;

namespace CSharpNow.Creational.Singleton.Modern
{
    /// <summary>
    /// Modern Singleton Pattern - Using Lazy<T>
    /// Provides lazy initialization while maintaining thread safety.
    /// Useful when initialization is expensive and might not always be needed.
    /// </summary>
    public class SingletonWithLazyT
    {
        // Lazy<T> handles all thread safety and initialization
        private static readonly Lazy<SingletonWithLazyT> _instance = 
            new Lazy<SingletonWithLazyT>(() => new SingletonWithLazyT());

        // Private constructor prevents instantiation
        private SingletonWithLazyT()
        {
            Console.WriteLine("SingletonWithLazyT initialized");
        }

        public static SingletonWithLazyT Instance => _instance.Value;

        public void DoSomething()
        {
            Console.WriteLine($"Lazy<T> Singleton instance: {this.GetHashCode()}");
        }
    }

    /// <summary>
    /// Usage example
    /// </summary>
    public class LazyTExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Singleton with Lazy<T> ===");
            Console.WriteLine("\nNotice that the instance is NOT initialized yet...");

            // Access instance - initialization happens here
            Console.WriteLine("\nAccessing instance for the first time:");
            var instance1 = SingletonWithLazyT.Instance;
            
            Console.WriteLine("\nAccessing instance again (already initialized):");
            var instance2 = SingletonWithLazyT.Instance;
            var instance3 = SingletonWithLazyT.Instance;

            instance1.DoSomething();
            instance2.DoSomething();
            instance3.DoSomething();

            Console.WriteLine($"\nAll instances are the same: {instance1.GetHashCode() == instance2.GetHashCode() && instance2.GetHashCode() == instance3.GetHashCode()}");
        }
    }
}
