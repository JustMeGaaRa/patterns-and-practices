using System;
using System.Collections.Generic;

namespace Silent.Practices.DDD
{
    public abstract class EventAggregate<TKey> : Entity<TKey>, IArchivable
    {
        private readonly List<Event<TKey>> _uncommittedChanges = new List<Event<TKey>>();
        private readonly Dictionary<Type, Action<Event<TKey>>> _eventHandlers = new Dictionary<Type, Action<Event<TKey>>>();

        public EventAggregate()
        {
            RegisterEventHandler<EventAggregateArchivedEvent<TKey>>(x => InternalSetArchived());
        }

        public bool IsArchived { get; private set; }

        public void Archive()
        {
            InternalSetArchived();
            AddUncommitedEvent(new EventAggregateArchivedEvent<TKey>(EntityId));
        }

        public IReadOnlyCollection<Event<TKey>> GetUncommitted()
        {
            return _uncommittedChanges;
        }

        public void MarkAsCommitted()
        {
            _uncommittedChanges.Clear();
        }

        public void ApplyHistory(IEnumerable<Event<TKey>> historicalEvents)
        {
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (Event<TKey> historyEvent in historicalEvents)
            {
                // NOTE: propagates generic TEvent type as base type
                ApplyEvent(historyEvent, false);
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : Event<TKey>
        {
            Action<Event<TKey>> genericHandler = eventInstance => handler.Invoke((TEvent)eventInstance);
            _eventHandlers[typeof(TEvent)] = genericHandler;
        }

        protected void AddUncommitedEvent<TEvent>(TEvent instance) where TEvent : Event<TKey>
        {
            _uncommittedChanges.Add(instance);
        }

        protected void ApplyEvent<TEvent>(TEvent instance) where TEvent : Event<TKey>
        {
            ApplyEvent(instance, true);
        }

        private void ApplyEvent<TEvent>(TEvent instance, bool isNew) where TEvent : Event<TKey>
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "Event instance cannot be null.");
            }

            if (isNew)
            {
                _uncommittedChanges.Add(instance);
            }

            // NOTE: instance.GetType() should not be changed to typeof(TEvent)
            // typeof(TEvent) returns base instance type
            // instance.GetType() returns actual instance type (is polymorphic)
            if (!_eventHandlers.ContainsKey(instance.GetType()))
            {
                throw new NotSupportedException($"Event of type '{typeof(TEvent)}' cannot be handled because no handler was registered.");
            }

            _eventHandlers[instance.GetType()].Invoke(instance);
        }

        private void InternalSetArchived()
        {
            IsArchived = true;
        }
    }

    public abstract class EventAggregate : EventAggregate<Guid>
    {
        protected EventAggregate() => EntityId = Guid.NewGuid();
    }
}