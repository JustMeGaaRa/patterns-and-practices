namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeEventAggregate : EventAggregate<uint>
    {
        public FakeEventAggregate()
        {
            RegisterHandler<FakeCreatedEvent>(x =>
            {
                var fakeCreatedEvent = x as FakeCreatedEvent;
                if (fakeCreatedEvent != null)
                {
                    Id = fakeCreatedEvent.AggregateId;
                }
            });
            RegisterHandler<FakeValueUpdatedEvent>(x => { });
        }

        public FakeEventAggregate(uint eventAggregateId, string value) : this()
        {
            Apply(new FakeCreatedEvent { AggregateId = eventAggregateId, Value = value });
        }

        public void ChangeValue(string newValue)
        {
            Apply(new FakeValueUpdatedEvent { AggregateId = Id, NewValue = newValue });
        }
    }
}