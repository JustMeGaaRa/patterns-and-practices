using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeValueUpdatedEvent : EventWithGuidKey
    {
        public FakeValueUpdatedEvent(Guid entityId) : base(entityId)
        {
        }

        public string NewValue { get; set; }
    }
}