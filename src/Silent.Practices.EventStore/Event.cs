using System;

namespace Silent.Practices.EventStore
{
    public abstract class Event
    {
        protected Event()
        {
            Timestamp = DateTime.UtcNow;
        }

        protected Event(uint entityId)
        {
            EntityId = entityId;
            Timestamp = DateTime.UtcNow;
        }

        public uint EntityId { get; }

        public DateTime Timestamp { get; }
    }
}
