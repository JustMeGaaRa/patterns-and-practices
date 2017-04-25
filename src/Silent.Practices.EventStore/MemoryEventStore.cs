using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore : IEventStore
    {
        private readonly Dictionary<uint, List<Event>> _events = new Dictionary<uint, List<Event>>();

        public IReadOnlyCollection<Event> GetEventsById(uint eventAggregateId)
        {
            IReadOnlyCollection<Event> events = _events.ContainsKey(eventAggregateId)
                ? _events[eventAggregateId].OrderBy(x => x.Timestamp).ToList()
                : new List<Event>();

            return events;
        }

        public IReadOnlyCollection<Event> GetEvents(Func<Event, bool> filter = null)
        {
            IReadOnlyCollection<Event> events = filter != null
                ? _events.Values.SelectMany(x => x).Where(filter).OrderBy(x => x.Timestamp).ToList()
                : new List<Event>();

            return events;
        }

        public bool SaveEvents(uint eventAggregateId, IReadOnlyCollection<Event> unsavedChanges)
        {
            if (!_events.ContainsKey(eventAggregateId))
            {
                _events.Add(eventAggregateId, new List<Event>());
            }

            _events[eventAggregateId].AddRange(unsavedChanges);
            return true;
        }
    }
}
