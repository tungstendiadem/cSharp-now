namespace cSharpNow.Behavioral.Observer;

/// <summary>
/// Subject interface that defines the contract for objects that notify observers
/// about state changes.
/// </summary>
public interface ISubject
{
    /// <summary>
    /// Attaches an observer to the subject.
    /// </summary>
    /// <param name="observer">The observer to attach.</param>
    void Attach(IObserver observer);

    /// <summary>
    /// Detaches an observer from the subject.
    /// </summary>
    /// <param name="observer">The observer to detach.</param>
    void Detach(IObserver observer);

    /// <summary>
    /// Notifies all observers about state changes.
    /// </summary>
    void Notify();
}
