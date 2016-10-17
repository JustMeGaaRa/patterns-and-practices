using System.Collections.Generic;
using System.Linq;
using Silent.Practices.Diagnostics;

namespace Silent.Practices.EventStore
{
    public class MemoryEvenAggregateRepository<TEntity> : 
        IEventAggregateRepository<TEntity>
        where TEntity : EventAggregate<uint>, new()
    {
        private readonly IEventStore _eventStore;

        public MemoryEvenAggregateRepository(IEventStore eventStore)
        {
            Contract.NotNull(eventStore, nameof(eventStore));
            _eventStore = eventStore;
        }

        public TEntity GetById(uint id)
        {
            TEntity eventAggregate = new TEntity();
            eventAggregate.ApplyHistory(_eventStore.GetEventsById(id));
            return eventAggregate;
        }

        public bool Save(TEntity item)
        {
            Contract.NotNull(item, nameof(item));

            IEnumerable<Event> uncommitted = item.GetUncommitted().ToList();
            return uncommitted.Any() && _eventStore.SaveEvents(item.Id, uncommitted);
        }
    }
}