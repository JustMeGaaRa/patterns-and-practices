using System;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore
{
    public abstract class EventAggregate<TKey> : EntityBase<TKey>
    {
        private readonly List<Event> _uncommittedChanges = new List<Event>();

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
                ApplyEvent(historyEvent, false);
            }
        }

        protected void ApplyEvent(Event instance)
        {
            ApplyEvent(instance, true);
        }

        private void ApplyEvent(Event instance, bool isNew)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "Event instance cannot be null.");
            }

            if (isNew)
            {
                _uncommittedChanges.Add(instance);
            }
        }
    }
}