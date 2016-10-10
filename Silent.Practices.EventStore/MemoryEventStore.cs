using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore : IEventStore
    {
        private readonly Dictionary<int, List<Event>> _events = new Dictionary<int, List<Event>>();

        public IEnumerable<Event> GetEventsById(int eventAggregateId)
        {
            if (!_events.ContainsKey(eventAggregateId))
            {
                throw new KeyNotFoundException($"Event agregate with given key is not present");
            }

            return _events[eventAggregateId];
        }

        public bool SaveEvents(int eventAggregateId, IEnumerable<Event> unsavedChanges)
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
