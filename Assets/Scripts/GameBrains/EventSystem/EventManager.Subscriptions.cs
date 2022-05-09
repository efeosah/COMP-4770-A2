namespace GameBrains.EventSystem
{
    /// <summary>
    /// Manager for events. Events can be fired for immediate processing or queued for later
    /// processing. Objects that subscribe to an event are notified when it is processed via its
    /// event delegate. Objects cease to be notified when they unsubscribe from an event.
    /// </summary>
    public sealed partial class EventManager
    {
        /// <summary>
        /// An event subscription record.
        /// </summary>
        struct Subscription
        {
            /// <summary>
            /// The event delegate.
            /// </summary>
            public System.Delegate EventDelegate;

            /// <summary>
            /// Gets the key used to identify subscriptions.
            /// </summary>
            public object EventKey;

            /// <summary>
            /// Initializes a new instance of the Subscription struct.
            /// </summary>
            /// <param name="eventDelegate">The event delegate.</param>
            /// <param name="eventKey">The event key.</param>
            public Subscription(System.Delegate eventDelegate, object eventKey)
                : this()
            {
                EventDelegate = eventDelegate;
                EventKey = eventKey;
            }
        }
    }
}