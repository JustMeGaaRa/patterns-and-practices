using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.DDD
{
    public abstract class EventAggregate<TKey, TEventBase> : EntityBase<TKey>
    {
        private readonly List<TEventBase> _uncommittedChanges = new List<TEventBase>();
        private readonly Dictionary<Type, Action<TEventBase>> _eventHandlers = new Dictionary<Type, Action<TEventBase>>();

        public IReadOnlyCollection<TEventBase> GetUncommitted()
        {
            return _uncommittedChanges;
        }

        public void MarkAsCommitted()
        {
            _uncommittedChanges.Clear();
        }

        public void ApplyHistory(IEnumerable<TEventBase> historicalEvents)
        {
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (TEventBase historyEvent in historicalEvents)
            {
                // NOTE: propagates generic TEvent type as base type
                ApplyEvent(historyEvent, false);
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : TEventBase
        {
            Action<TEventBase> genericHandler = eventInstance => handler.Invoke((TEvent)eventInstance);
            _eventHandlers[typeof(TEvent)] = genericHandler;
        }

        protected void ApplyEvent<TEvent>(TEvent instance) where TEvent : TEventBase
        {
            ApplyEvent(instance, true);
        }

        private void ApplyEvent<TEvent>(TEvent instance, bool isNew) where TEvent : TEventBase
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