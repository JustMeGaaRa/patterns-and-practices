using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeCreatedEvent : EventWithGuidKey
    {
        public FakeCreatedEvent(Guid entityId, long version) : base(entityId, version)
        {
        }

        public string Value { get; set; }
    }
}