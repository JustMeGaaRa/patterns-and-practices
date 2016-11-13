namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeEventAggregate : EventAggregate<uint>
    {
        private string _value;

        public FakeEventAggregate()
        {
            OnEvent<FakeCreatedEvent>(x => Id = x.EntityId);
            OnEvent<FakeValueUpdatedEvent>(x => _value = x.NewValue);
        }

        public FakeEventAggregate(uint eventAggregateId) : this()
        {
            Apply(new FakeCreatedEvent(eventAggregateId));
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (Value != value)
                {
                    Apply(new FakeValueUpdatedEvent(Id) { NewValue = value });
                }
            }
        }
    }
}