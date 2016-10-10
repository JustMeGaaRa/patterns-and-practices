using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore
{
    public abstract class EventAggregate<TKey> : EntityBase<TKey>
    {
        private readonly Dictionary<Type, Action<Event>> _actualHandlers = new Dictionary<Type, Action<Event>>();
        private readonly List<Event> _uncommittedChanges = new List<Event>();

        public IEnumerable<Event> GetUncommitted()
        {
            return _uncommittedChanges;
        }

        public void MarkAsCommitted()
        {
            _uncommittedChanges.Clear();
        }

        public void ApplyHistory(IEnumerable<Event> historicalEvents)
        {
            foreach (Event historyEvent in historicalEvents)
            {
                Apply(historyEvent, false);
            }
        }

        protected void Apply<TEvent>(TEvent instance) where TEvent : Event
        {
            Apply(instance, true);
        }

        private void Apply<TEvent>(TEvent instance, bool isNew) where TEvent : Event
        {
            Type eventType = typeof(TEvent);

            if (!_actualHandlers.ContainsKey(eventType))
            {
                throw new NotSupportedException($"No handler of type {eventType.FullName} is present");
            }

            _actualHandlers[eventType].Invoke(instance);

            if (isNew)
            {
                _uncommittedChanges.Add(instance);
            }
        }
    }
}