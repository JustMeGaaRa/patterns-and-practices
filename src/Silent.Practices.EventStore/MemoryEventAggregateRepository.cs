using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.EventStore
{
    public class MemoryEventAggregateRepository<TEntity> : 
        IEventAggregateRepository<TEntity>
        where TEntity : EventAggregate<uint>, new()
    {
        private readonly IEventStore _eventStore;

        public MemoryEventAggregateRepository(IEventStore eventStore)
        {
            if (eventStore == null)
            {
                throw new ArgumentNullException(nameof(eventStore));
            }

            _eventStore = eventStore;
        }

        public TEntity GetById(uint id)
        {
            TEntity eventAggregate = null;
            IEnumerable<Event> committed = _eventStore.GetEventsById(id);

            if (committed != null && committed.Any())
            {
                eventAggregate = new TEntity();
                eventAggregate.ApplyHistory(committed);
            }
            
            return eventAggregate;
        }

        public bool Save(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            IReadOnlyCollection<Event> uncommitted = item.GetUncommitted();
            return uncommitted.Any() && _eventStore.SaveEvents(item.Id, uncommitted);
        }
    }
}