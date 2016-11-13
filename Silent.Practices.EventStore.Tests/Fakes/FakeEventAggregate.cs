namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeEventAggregate : EventAggregate<uint>
    {
        public FakeEventAggregate()
        {
        }

        public FakeEventAggregate(uint eventAggregateId) : this()
        {
            ApplyEvent(new FakeCreatedEvent(eventAggregateId));
        }
    }
}