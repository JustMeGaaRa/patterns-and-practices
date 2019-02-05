using System;

namespace Silent.Practices.DDD
{
    public abstract class Event<TKey>
    {
        protected Event(TKey entityId, long version)
        {
            EventId = Guid.NewGuid();
            EntityId = entityId;
            Version = version;
            Timestamp = DateTime.UtcNow;
        }

        public Guid EventId { get; set; }

        public TKey EntityId { get; set; }

        public long Version { get; }

        public DateTime Timestamp { get; }
    }

    public abstract class EventWithGuidKey : Event<Guid>
    {
        protected EventWithGuidKey(Guid entityId, long version) : base(entityId, version)
        {
        }
    }

    public abstract class EventWithIntKey : Event<int>
    {
        protected EventWithIntKey(int entityId, long version) : base(entityId, version)
        {
        }
    }
}
