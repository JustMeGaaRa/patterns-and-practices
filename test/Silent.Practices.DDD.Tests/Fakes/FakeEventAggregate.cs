namespace Silent.Practices.DDD.Tests.Fakes
{
    internal sealed class FakeEventAggregate : EventAggregate<uint, Event>
    {
        public FakeEventAggregate()
        {
            RegisterEventHandler<FakeCreatedEvent>(x => Id = x.EntityId);
            RegisterEventHandler<FakeValueUpdatedEvent>(x => Value = x.NewValue);
        }
        
        public FakeEventAggregate(uint eventAggregateId) : this()
        {
            ApplyEvent(new FakeCreatedEvent(eventAggregateId));
        }

        public string Value { get; private set; }

        public void SetValue(string newValue)
        {
            ApplyEvent(new FakeValueUpdatedEvent(Id) { NewValue = newValue });
        }
    }
}