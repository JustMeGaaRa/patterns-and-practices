namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeCreatedEvent : Event
    {
        public uint AggregateId { get; set; }

        public string Value { get; set; }
    }
}