using System;
using System.Collections.Generic;
using System.Linq;
using Silent.Practices.EventStore;

namespace Silent.Practices.DDD
{
    public class MemoryEventAggregateRepository<TEntity, TKey, TEventBase> : IEventAggregateRepository<TEntity, TKey, TEventBase>
        where TEntity : EventAggregate<TKey, TEventBase>, new()
        where TEventBase : Event<TKey>
    {
        private readonly IEventStore<TKey, TEventBase> _eventStore;

        public MemoryEventAggregateRepository(IEventStore<TKey, TEventBase> eventStore)
        {
            if (eventStore == null)
            {
                throw new ArgumentNullException(nameof(eventStore));
            }

            _eventStore = eventStore;
        }

        public virtual TEntity GetById(TKey id)
        {
            TEntity eventAggregate = null;
            IEnumerable<TEventBase> committed = _eventStore.GetEventsById(id);

            if (committed != null && committed.Any())
            {
                eventAggregate = new TEntity();
                eventAggregate.ApplyHistory(committed);
            }
            
            return eventAggregate;
        }

        public virtual ICollection<TEntity> Get()
        {
            return _eventStore.GetEvents()
                .GroupBy(x => x.EntityId)
                .Select(x => GetById(x.Key))
                .ToList();
        }

        public virtual bool Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Update(entity.Id, entity);
        }

        public virtual bool Update(TKey key, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            IReadOnlyCollection<TEventBase> uncommitted = entity.GetUncommitted();
            return uncommitted.Any() && _eventStore.SaveEvents(key, uncommitted);
        }

        public virtual bool Delete(TKey key)
        {
            throw new NotImplementedException();
        }
    }

    public class MemoryEventAggregateRepository<TEntity> : 
        MemoryEventAggregateRepository<TEntity, uint, Event<uint>>, 
        IEventAggregateRepository<TEntity>
        where TEntity : EventAggregate<uint, Event<uint>>, new()
    {
        public MemoryEventAggregateRepository(IEventStore<uint, Event<uint>> eventStore) : base(eventStore)
        {
        }
    }
}