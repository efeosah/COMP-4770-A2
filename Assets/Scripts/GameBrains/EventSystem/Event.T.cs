using GameBrains.Entities;

namespace GameBrains.EventSystem
{
   /// <summary>
    /// The base class for events.
    /// </summary>
    /// <typeparam name="T">
    /// The type of event data.
    /// </typeparam>
    public sealed class Event<T> : Event
    {
		/// <summary>
        /// Initializes a new instance of the Event class.
        /// </summary>
        /// <param name="eventId">
        /// The event ID.
        /// </param>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="lifespan">
        /// The maximum duration of the event.
        /// </param>
        /// <param name="dispatchTime">
        /// The time to dispatch the event (or DISPATCH_IMMEDIATELY).
        /// </param>
        /// <param name="senderId">
        /// The sender ID (or SENDER_ID_IRRELEVANT).
        /// </param>
        /// <param name="receiverId">
        /// The receiver ID (or RECEIVER_ID_IRRELEVANT).
        /// </param>
        /// <param name="eventDelegate">
        /// The delegate to call when the event is triggered.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        private Event(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData)
            : base(eventId, eventType, lifespan, dispatchTime, senderId, receiverId, eventDelegate, typeof(T), eventData)
        {
        }

		Event()
        {
        }

        /// <summary>
        /// Gets the event data (may be null).
        /// </summary>
        public new T EventData
        {
            get => (T)base.EventData;

            private set => base.EventData = value;
        }

        /// <summary>
        /// Gets the delegate to call when the event is triggered.
        /// </summary>
        public new EventDelegate<T> EventDelegate
        {
            get => (EventDelegate<T>)base.EventDelegate;

            private set => base.EventDelegate = value;
        }
		
		public static Event<T> Obtain(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            EventDelegate<T> eventDelegate,
            T eventData)
        {
			// TODO: make events poolable to reduce garbage
            return new Event<T>(
                    eventId,
                    eventType,
                    lifespan,
                    dispatchTime,
                    senderId,
                    receiverId,
                    eventDelegate,
                    eventData);
		}
		
		/// <summary>
        /// Returns a System.String that represents the event.
        /// </summary>
        /// <returns>A System.String that represents the event.</returns>
        public override string ToString()
        {
            return string.Format(
                "Id:{0}, Type:{1}, Lifespan:{2} Sender:{3}, Receiver:{4}, Data:{5}",
                EventId,
                EventType,
                EventLifespan,
                SenderId,
                ReceiverId,
                EventData);
        }

        /// <summary>
        /// Trigger event.
        /// </summary>
        /// <param name="delegateToFire">
        /// The event delegate to fire.
        /// </param>
        internal override void Fire(System.Delegate delegateToFire)
        {
	        var eventDelegate = delegateToFire as EventDelegate<T>;

	        if (eventDelegate != null)
            {
                eventDelegate(this);
            }
        }

        internal override void Send()
        {
			if (ReceiverId != EventManager.RECEIVER_ID_IRRELEVANT)
			{
				Entity entity = EntityManager.Find<Entity>(ReceiverId);
				if (entity != null)
				{
					entity.HandleEvent(this);
				}
			}
			else
			{
				foreach (var messageViewer in EventManager.Instance.DefaultMessageViewers)
				{
					messageViewer.HandleEvent(this);
				}
			}
        }
	}
}