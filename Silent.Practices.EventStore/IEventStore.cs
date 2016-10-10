using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore
    {
        IEnumerable<Event> GetEventsById(int eventAggregateId);

        bool SaveEvents(int eventAggregateId, IEnumerable<Event> unsavedChanges);
    }
}
