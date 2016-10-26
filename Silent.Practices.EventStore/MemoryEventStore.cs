using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public class MemoryEventStore : IEventStore
    {
        private readonly Dictionary<uint, List<Event>> _events = new Dictionary<uint, List<Event>>();

        public ICollection<Event> GetEventsById(uint eventAggregateId)
        {
            ICollection<Event> events = _events.ContainsKey(eventAggregateId)
                ? _events[eventAggregateId]
                : new List<Event>();

            return events;
        }

        public bool SaveEvents(uint eventAggregateId, ICollection<Event> unsavedChanges)
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
