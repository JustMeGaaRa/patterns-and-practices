using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore : IEventStore
    {
        private readonly Dictionary<uint, List<Event>> _events = new Dictionary<uint, List<Event>>();

        public IReadOnlyCollection<Event> GetEventsById(uint eventAggregateId)
        {
            IReadOnlyCollection<Event> events = _events.ContainsKey(eventAggregateId)
                ? _events[eventAggregateId]
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
