using System;

namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeEventAggregate : EventAggregate
    {
        public FakeEventAggregate()
        {
            RegisterEventHandler<FakeCreatedEvent>(x => EntityId = x.EntityId);
            RegisterEventHandler<FakeValueUpdatedEvent>(x => Value = x.NewValue);
        }
        
        public FakeEventAggregate(Guid eventAggregateId) : this()
        {
            ApplyEvent(new FakeCreatedEvent(eventAggregateId, GetNextAggregateVersion()));
        }

        public string Value { get; private set; }

        public void SetValue(string newValue)
        {
            ApplyEvent(new FakeValueUpdatedEvent(EntityId, GetNextAggregateVersion()) { NewValue = newValue });
        }
    }
}