namespace Silent.Practices.EventStore.Tests.Fakes
{
    internal sealed class FakeDeletedEvent : Event
    {
        public FakeDeletedEvent(uint id) : base(id)
        {
        }
    }
}