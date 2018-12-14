using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeDeletedEvent : EventWithGuidKey
    {
        public FakeDeletedEvent(Guid entityId) : base(entityId)
        {
        }
    }
}