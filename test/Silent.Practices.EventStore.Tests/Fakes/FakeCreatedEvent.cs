namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeCreatedEvent : Event
    {
        public FakeCreatedEvent(uint id) : base(id)
        {
        }

        public string Value { get; set; }
    }
}