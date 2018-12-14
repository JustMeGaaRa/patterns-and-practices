using Silent.Practices.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.DDD
{
    public class MemoryEventAggregateRepository<TEntity> :
        IEventSourcedRepository<TEntity>
        where TEntity : EventAggregate, new()
    {
        private readonly IEventStore<Guid, EventWithGuidKey> _eventStore;

        public MemoryEventAggregateRepository(IEventStore<Guid, EventWithGuidKey> eventStore)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public virtual TEntity FindById(Guid id)
        {
            TEntity eventAggregate = null;
            IEnumerable<EventWithGuidKey> committed = _eventStore.GetEventsById(id);

            if (committed != null && committed.Any())
            {
                eventAggregate = new TEntity();
                eventAggregate.ApplyHistory(committed);
            }
            
            return eventAggregate;
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return _eventStore.GetEvents()
                .GroupBy(x => x.EntityId)
                .Select(x => FindById(x.Key))
                .ToList();
        }

        public virtual bool Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Update(entity.EntityId, entity);
        }

        public virtual bool Update(Guid key, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            IReadOnlyCollection<EventWithGuidKey> uncommitted = entity.GetUncommitted();
            return uncommitted.Any() && _eventStore.SaveEvents(key, uncommitted);
        }

        public virtual bool DeleteById(Guid key)
        {
            FindById(key)?.Archive();
            return true;
        }

        public bool Delete(TEntity entity)
        {
            entity?.Archive();
            return true;
        }
    }
}