using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeValueUpdatedEvent : EventWithGuidKey
    {
        public FakeValueUpdatedEvent(Guid entityId, long version) : base(entityId, version)
        {
        }

        public string NewValue { get; set; }
    }
}