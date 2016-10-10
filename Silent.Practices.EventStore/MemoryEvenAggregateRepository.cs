using System;

namespace Silent.Practices.EventStore
{
    public class MemoryEvenAggregateRepository<TEntity> : 
        IEventAggregateRepository<TEntity>
        where TEntity : EventAggregate<int>, new()
    {
        private readonly IEventStore _eventStore;

        public MemoryEvenAggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public int Count
        {
            get { throw new NotSupportedException(); }
        }

        public TEntity GetById(int id)
        {
            TEntity eventAggregate = new TEntity();
            eventAggregate.ApplyHistory(_eventStore.GetEventsById(id));
            return eventAggregate;
        }

        public bool Save(TEntity item)
        {
            return _eventStore.SaveEvents(item.Id, item.GetUncommitted());
        }
    }
}