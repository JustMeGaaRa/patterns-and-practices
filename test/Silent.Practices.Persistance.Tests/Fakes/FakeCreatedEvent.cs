using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeCreatedEvent : EventWithGuidKey
    {
        public FakeCreatedEvent(Guid entityId) : base(entityId)
        {
        }

        public string Value { get; set; }
    }
}