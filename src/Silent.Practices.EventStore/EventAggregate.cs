using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore
{
    public abstract class EventAggregate<TKey> : EntityBase<TKey>
    {
        private readonly List<Event> _uncommittedChanges = new List<Event>();
        private readonly Dictionary<Type, Action<Event>> _eventHandlers = new Dictionary<Type, Action<Event>>();

        public IReadOnlyCollection<Event> GetUncommitted()
        {
            return _uncommittedChanges;
        }

        public void MarkAsCommitted()
        {
            _uncommittedChanges.Clear();
        }

        public void ApplyHistory(IEnumerable<Event> historicalEvents)
        {
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (Event historyEvent in historicalEvents)
            {
                // NOTE: propagates generic TEvent type as base type
                ApplyEvent(historyEvent, false);
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : Event
        {
            Action<Event> genericHandler = eventInstance => handler.Invoke(eventInstance as TEvent);
            _eventHandlers[typeof(TEvent)] = genericHandler;
        }

        protected void ApplyEvent<TEvent>(TEvent instance) where TEvent : Event
        {
            ApplyEvent(instance, true);
        }

        private void ApplyEvent<TEvent>(TEvent instance, bool isNew) where TEvent : Event
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
    }
}