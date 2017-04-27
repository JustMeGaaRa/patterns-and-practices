using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore<TKey, TEvent> : IEventStore<TKey, TEvent>
    {
        private readonly IComparer<TEvent> _comparer;
        private readonly Dictionary<TKey, List<TEvent>> _events = new Dictionary<TKey, List<TEvent>>();

        public MemoryEventStore(IComparer<TEvent> comparer)
        {
            _comparer = comparer;
        }

        public IReadOnlyCollection<TEvent> GetEventsById(TKey eventAggregateId)
        {
            IReadOnlyCollection<TEvent> events = _events.ContainsKey(eventAggregateId)
                ? _events[eventAggregateId].OrderBy(x => x, _comparer).ToList()
                : new List<TEvent>();

            return events;
        }

        public IReadOnlyCollection<TEvent> GetEvents(Func<TEvent, bool> filter = null)
        {
            IReadOnlyCollection<TEvent> events = filter != null
                ? _events.Values.SelectMany(x => x).Where(filter).OrderBy(x => x, _comparer).ToList()
                : new List<TEvent>();

            return events;
        }

        public bool SaveEvents(TKey eventAggregateId, IReadOnlyCollection<TEvent> unsavedChanges)
        {
            if (!_events.ContainsKey(eventAggregateId))
            {
                _events.Add(eventAggregateId, new List<TEvent>());
            }

            _events[eventAggregateId].AddRange(unsavedChanges);
            return true;
        }
    }
}
