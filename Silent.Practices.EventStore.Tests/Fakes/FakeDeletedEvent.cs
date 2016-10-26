namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeDeletedEvent : Event
    {
        public uint AggregateId { get; set; }
    }
}