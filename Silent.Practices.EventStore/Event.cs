namespace Silent.Practices.EventStore
{
    public abstract class Event
    {
        public int Version { get; set; }
    }
}
