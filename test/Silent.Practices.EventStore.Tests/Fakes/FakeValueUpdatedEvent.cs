namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeValueUpdatedEvent : Event
    {
        public FakeValueUpdatedEvent(uint id) : base(id)
        {
        }

        public string NewValue { get; set; }
    }
}