using System;

namespace Silent.Practices.DDD
{
    public class EventAggregateArchivedEvent : EventWithGuidKey
    {
        public EventAggregateArchivedEvent(Guid entityId) : base(entityId)
        {
        }
    }
}