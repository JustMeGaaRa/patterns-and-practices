namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeValueUpdatedEvent : Event
    {
        public uint AggregateId { get; set; }

        public string NewValue { get; set; }
    }
}