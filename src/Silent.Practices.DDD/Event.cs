using System;

namespace Silent.Practices.DDD
{
    public abstract class Event<TKey>
    {
        protected Event(TKey entityId)
        {
            EventId = Guid.NewGuid();
            EntityId = entityId;
            Timestamp = DateTime.UtcNow;
        }

        public Guid EventId { get; set; }

        public TKey EntityId { get; set; }

        public DateTime Timestamp { get; }
    }

    public abstract class EventWithGuidKey : Event<Guid>
    {
        protected EventWithGuidKey(Guid entityId) : base(entityId)
        {
        }
    }

    public abstract class EventWithIntKey : Event<int>
    {
        protected EventWithIntKey(int entityId) : base(entityId)
        {
        }
    }
}
