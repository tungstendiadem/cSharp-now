namespace cSharpNow.Behavioral.Observer;

/// <summary>
/// Concrete Subject that maintains state and notifies observers when the state changes.
/// </summary>
public class ConcreteSubject : ISubject
{
    private List<IObserver> _observers = new();
    private string _state;

    public string State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Attaches an observer to the subject.
    /// </summary>
    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"Observer attached. Total observers: {_observers.Count}");
        }
    }

    /// <summary>
    /// Detaches an observer from the subject.
    /// </summary>
    public void Detach(IObserver observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($"Observer detached. Total observers: {_observers.Count}");
        }
    }

    /// <summary>
    /// Notifies all observers about the state change.
    /// </summary>
    public void Notify()
    {
        Console.WriteLine($"\nSubject: State changed to: {_state}");
        Console.WriteLine("Subject: Notifying observers...\n");

        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}
