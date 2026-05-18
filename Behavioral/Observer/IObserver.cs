namespace cSharpNow.Behavioral.Observer;

/// <summary>
/// Observer interface that defines the contract for objects that want to be notified
/// of changes in a Subject.
/// </summary>
public interface IObserver
{
    /// <summary>
    /// Called when the Subject's state changes.
    /// </summary>
    /// <param name="subject">The subject that changed.</param>
    void Update(ISubject subject);
}
