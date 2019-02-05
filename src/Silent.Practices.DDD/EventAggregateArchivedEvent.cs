namespace Silent.Practices.DDD
{
    public class EventAggregateArchivedEvent<TKey> : Event<TKey>
    {
        public EventAggregateArchivedEvent(TKey entityId, long version) : base(entityId, version)
        {
        }
    }
}