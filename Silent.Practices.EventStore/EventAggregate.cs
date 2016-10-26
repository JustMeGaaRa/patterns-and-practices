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
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (Event historyEvent in historicalEvents)
            {
                Apply(historyEvent, false);
            }
        }

        protected void RegisterHandler<TEvent>(Action<Event> handler)
        {
            Type eventType = typeof(TEvent);
            _actualHandlers[eventType] = handler;
        }

        protected void Apply(Event instance)
        {
            Apply(instance, true);
        }

        private void Apply(Event instance, bool isNew)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "Event instance cannot be null.");
            }

            Type eventType = instance.GetType();

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