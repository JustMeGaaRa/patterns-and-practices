using System;
using System.Collections.Generic;

namespace Silent.Practices.DDD
{
    public abstract class EventAggregate : EntityWithGuidKey, IArchivable
    {
        private readonly List<EventWithGuidKey> _uncommittedChanges = new List<EventWithGuidKey>();
        private readonly Dictionary<Type, Action<EventWithGuidKey>> _eventHandlers = new Dictionary<Type, Action<EventWithGuidKey>>();

        public EventAggregate()
        {
            RegisterEventHandler<EventAggregateArchivedEvent>(x => InternalSetArchived());
        }

        public bool IsArchived { get; private set; }

        public void Archive()
        {
            InternalSetArchived();
            AddUncommitedEvent(new EventAggregateArchivedEvent(EntityId));
        }

        public IReadOnlyCollection<EventWithGuidKey> GetUncommitted()
        {
            return _uncommittedChanges;
        }

        public void MarkAsCommitted()
        {
            _uncommittedChanges.Clear();
        }

        public void ApplyHistory(IEnumerable<EventWithGuidKey> historicalEvents)
        {
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (EventWithGuidKey historyEvent in historicalEvents)
            {
                // NOTE: propagates generic TEvent type as base type
                ApplyEvent(historyEvent, false);
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : EventWithGuidKey
        {
            Action<EventWithGuidKey> genericHandler = eventInstance => handler.Invoke((TEvent)eventInstance);
            _eventHandlers[typeof(TEvent)] = genericHandler;
        }

        protected void AddUncommitedEvent<TEvent>(TEvent instance) where TEvent : EventWithGuidKey
        {
            _uncommittedChanges.Add(instance);
        }

        protected void ApplyEvent<TEvent>(TEvent instance) where TEvent : EventWithGuidKey
        {
            ApplyEvent(instance, true);
        }

        private void ApplyEvent<TEvent>(TEvent instance, bool isNew) where TEvent : EventWithGuidKey
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
}