using System;

namespace CSharpNow.Creational.Singleton.Classic
{
    /// <summary>
    /// Classic Singleton Pattern - Traditional Gang of Four Implementation
    /// This is the pattern most commonly described in design pattern books.
    /// </summary>
    public class ClassicSingleton
    {
        // Static instance
        private static ClassicSingleton _instance;

        // Static object for thread locking
        private static readonly object _lock = new object();

        // Private constructor prevents instantiation
        private ClassicSingleton()
        {
        }

        /// <summary>
        /// Get the singleton instance using double-checked locking
        /// </summary>
        public static ClassicSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ClassicSingleton();
                        }
                    }
                }
                return _instance;
            }
        }

        public void DoSomething()
        {
            Console.WriteLine($"Classic Singleton instance: {this.GetHashCode()}");
        }
    }

    /// <summary>
    /// Usage example of Classic Singleton
    /// </summary>
    public class ClassicSingletonExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Classic Singleton Pattern ===");

            // Get instance multiple times
            var instance1 = ClassicSingleton.Instance;
            var instance2 = ClassicSingleton.Instance;
            var instance3 = ClassicSingleton.Instance;

            instance1.DoSomething();
            instance2.DoSomething();
            instance3.DoSomething();

            // All should have the same hash code (same instance)
            Console.WriteLine($"\nAll instances are the same: {instance1.GetHashCode() == instance2.GetHashCode() && instance2.GetHashCode() == instance3.GetHashCode()}");
        }
    }
}
