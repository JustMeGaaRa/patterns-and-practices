using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore : IEventStore
    {
        private readonly Dictionary<uint, List<Event>> _events = new Dictionary<uint, List<Event>>();

        public IEnumerable<Event> GetEventsById(uint eventAggregateId)
        {
            if (!_events.ContainsKey(eventAggregateId))
            {
                throw new KeyNotFoundException($"Event aggregate with id '{eventAggregateId}' is not present");
            }

            return _events[eventAggregateId];
        }

        public bool SaveEvents(uint eventAggregateId, IEnumerable<Event> unsavedChanges)
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
