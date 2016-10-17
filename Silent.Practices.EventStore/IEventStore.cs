using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore
    {
        IEnumerable<Event> GetEventsById(uint eventAggregateId);

        bool SaveEvents(uint eventAggregateId, IEnumerable<Event> unsavedChanges);
    }
}
