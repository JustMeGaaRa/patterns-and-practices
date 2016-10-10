namespace Silent.Practices.EventStore
{
    public interface IEventPublisher
    {
        void Publish(Event instance);
    }
}
