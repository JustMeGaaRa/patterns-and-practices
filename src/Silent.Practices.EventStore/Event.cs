namespace Silent.Practices.EventStore
{
    public abstract class Event
    {
        protected Event()
        {
        }

        protected Event(uint entityId)
        {
            EntityId = entityId;
        }

        public uint EntityId { get; }
    }
}
