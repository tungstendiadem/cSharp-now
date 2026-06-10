using System;

namespace DecoratorPattern
{
    // Component interface
    public interface IComponent
    {
        string GetDescription();
        decimal GetCost();
    }

    // ConcreteComponent - Base coffee
    public class SimpleCoffee : IComponent
    {
        public string GetDescription()
        {
            return "Simple Coffee";
        }

        public decimal GetCost()
        {
            return 2.0m;
        }
    }

    // Decorator abstract class
    public abstract class CoffeeDecorator : IComponent
    {
        protected IComponent _component;

        public CoffeeDecorator(IComponent component)
        {
            _component = component;
        }

        public virtual string GetDescription()
        {
            return _component.GetDescription();
        }

        public virtual decimal GetCost()
        {
            return _component.GetCost();
        }
    }

    // ConcreteDecorator - Milk addition
    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(IComponent component) : base(component)
        {
        }

        public override string GetDescription()
        {
            return base.GetDescription() + ", Milk";
        }

        public override decimal GetCost()
        {
            return base.GetCost() + 0.50m;
        }
    }

    // ConcreteDecorator - Sugar addition
    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(IComponent component) : base(component)
        {
        }

        public override string GetDescription()
        {
            return base.GetDescription() + ", Sugar";
        }

        public override decimal GetCost()
        {
            return base.GetCost() + 0.25m;
        }
    }

    // ConcreteDecorator - Whipped cream addition
    public class WhippedCreamDecorator : CoffeeDecorator
    {
        public WhippedCreamDecorator(IComponent component) : base(component)
        {
        }

        public override string GetDescription()
        {
            return base.GetDescription() + ", Whipped Cream";
        }

        public override decimal GetCost()
        {
            return base.GetCost() + 0.75m;
        }
    }

    // Client code
    class Program
    {
        static void Main(string[] args)
        {
            // Simple coffee
            IComponent coffee = new SimpleCoffee();
            Console.WriteLine($"Order: {coffee.GetDescription()}");
            Console.WriteLine($"Cost: ${coffee.GetCost()}\n");

            // Coffee with milk
            coffee = new MilkDecorator(coffee);
            Console.WriteLine($"Order: {coffee.GetDescription()}");
            Console.WriteLine($"Cost: ${coffee.GetCost()}\n");

            // Coffee with milk and sugar
            coffee = new SugarDecorator(coffee);
            Console.WriteLine($"Order: {coffee.GetDescription()}");
            Console.WriteLine($"Cost: ${coffee.GetCost()}\n");

            // Coffee with milk, sugar, and whipped cream
            coffee = new WhippedCreamDecorator(coffee);
            Console.WriteLine($"Order: {coffee.GetDescription()}");
            Console.WriteLine($"Cost: ${coffee.GetCost()}\n");

            // Create a different combination: Simple coffee with whipped cream
            IComponent simpleCoffeeWithWhip = new WhippedCreamDecorator(new SimpleCoffee());
            Console.WriteLine($"Order: {simpleCoffeeWithWhip.GetDescription()}");
            Console.WriteLine($"Cost: ${simpleCoffeeWithWhip.GetCost()}");
        }
    }
}
