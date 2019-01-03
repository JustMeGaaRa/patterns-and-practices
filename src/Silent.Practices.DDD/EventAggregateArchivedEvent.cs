using System;

namespace Silent.Practices.DDD
{
    public class EventAggregateArchivedEvent<TKey> : Event<TKey>
    {
        public EventAggregateArchivedEvent(TKey entityId) : base(entityId)
        {
        }
    }
}