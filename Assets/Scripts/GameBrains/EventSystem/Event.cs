namespace GameBrains.EventSystem
{
    public abstract partial class Event
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
        /// <param name="eventDataType">
        /// The type of event data.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        protected Event(
            int eventId,
            EventType eventType,
            Lifespan lifespan,
            double dispatchTime,
            int senderId,
            int receiverId,
            System.Delegate eventDelegate,
            System.Type eventDataType,
            object eventData)
        {
            EventId = eventId;
            EventType = eventType;
            EventLifespan = lifespan;
            DispatchTime = dispatchTime;
            SenderId = senderId;
            ReceiverId = receiverId;
            EventDelegate = eventDelegate;
            EventDataType = eventDataType;
            EventData = eventData;
        }

        protected internal Event()
        {
        }

        /// <summary>
        /// The event id for tracking the event.
        /// </summary>
        public int EventId { get; protected set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        public EventType EventType { get; protected set; }

        /// <summary>
        /// Gets or sets the maximum duration of the event.
        /// </summary>
        public Lifespan EventLifespan { get; protected set; }

        /// <summary>
        /// Gets or sets the time to dispatch the event. Events can be dispatched immediately, queued for the next
        /// processing cycle or delayed for a specified amount of time. If a delay is necessary this field is stamped
        /// with the time the event should be dispatched.
        /// </summary>
        public double DispatchTime { get; protected set; }

        /// <summary>
        /// Gets or sets the ID of the game object that sent this event (or Event.SENDER_ID_IRRELEVANT).
        /// </summary>
        public int SenderId { get; protected set; }

        /// <summary>
        /// Gets or sets the ID of the intended receiver of this event (or Event.RECEIVER_ID_IRRELEVANT).
        /// </summary>
        public int ReceiverId { get; protected set; }

        /// <summary>
        /// Gets or sets the type of event data.
        /// </summary>
        public System.Type EventDataType { get; protected set; }

        /// <summary>
        /// Gets or sets the event data (or null).
        /// </summary>
        public object EventData { get; protected set; }

        /// <summary>
        /// Gets or sets the delegate to call when the event is triggered.
        /// </summary>
        public System.Delegate EventDelegate { get; protected set; }

        /// <summary>
        /// Trigger event.
        /// </summary>
        /// <param name="eventDelegate">
        /// The event delegate.
        /// </param>
        internal abstract void Fire(System.Delegate eventDelegate);

        internal abstract void Send();
	}
}
