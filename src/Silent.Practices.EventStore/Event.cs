using System;

namespace Silent.Practices.EventStore
{
    public abstract class Event<TKey> : IEvent
    {
        protected Event()
        {
            Timestamp = DateTime.UtcNow;
        }

        protected Event(TKey entityId)
        {
            EntityId = entityId;
            Timestamp = DateTime.UtcNow;
        }

        public TKey EntityId { get; set; }

        public DateTime Timestamp { get; }
    }
}
