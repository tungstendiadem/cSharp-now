using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpNow.Creational.Singleton.Modern
{
    /// <summary>
    /// Modern Singleton Pattern - Using Dependency Injection
    /// This is the recommended approach in modern C# applications.
    /// The DI container manages the singleton lifecycle automatically.
    /// </summary>
    public interface IConfiguration
    {
        string GetSetting(string key);
    }

    public class AppConfiguration : IConfiguration
    {
        public AppConfiguration()
        {
            Console.WriteLine("AppConfiguration initialized");
        }

        public string GetSetting(string key)
        {
            return $"Setting: {key}";
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        private readonly IConfiguration _config;

        public ConsoleLogger(IConfiguration config)
        {
            _config = config;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
            Console.WriteLine($"Configuration instance: {_config.GetHashCode()}");
        }
    }

    /// <summary>
    /// Usage example
    /// </summary>
    public class DependencyInjectionExample
    {
        public static void Main()
        {
            Console.WriteLine("=== Singleton with Dependency Injection ===");

            // Setup dependency injection container
            var services = new ServiceCollection();
            
            // Register AppConfiguration as singleton
            services.AddSingleton<IConfiguration, AppConfiguration>();
            
            // Register ConsoleLogger as transient (creates new instance each time)
            services.AddTransient<ILogger, ConsoleLogger>();

            var serviceProvider = services.BuildServiceProvider();

            // Get logger instances - they're different
            Console.WriteLine("\nCreating first logger:");
            var logger1 = serviceProvider.GetRequiredService<ILogger>();
            logger1.Log("First log message");

            Console.WriteLine("\nCreating second logger:");
            var logger2 = serviceProvider.GetRequiredService<ILogger>();
            logger2.Log("Second log message");

            Console.WriteLine("\nCreating third logger:");
            var logger3 = serviceProvider.GetRequiredService<ILogger>();
            logger3.Log("Third log message");

            // Notice: Configuration instances are the same across all loggers!
            Console.WriteLine($"\nAll loggers share the same configuration (singleton)");
        }
    }
}
