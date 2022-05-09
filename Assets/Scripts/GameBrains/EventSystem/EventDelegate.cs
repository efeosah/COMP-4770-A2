namespace GameBrains.EventSystem
{
    /// <summary>
    /// Called whenever the associated Event fires.
    /// </summary>
    /// <typeparam name="T">
    /// The type of event data.
    /// </typeparam>
    /// <param name="eventT">
    /// The event.
    /// /// </param>
    public delegate void EventDelegate<T>(Event<T> eventT);

    public delegate bool MessageDelegate<T>(Event<T> eventT);
}