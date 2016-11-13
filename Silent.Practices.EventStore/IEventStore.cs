using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore
    {
        IReadOnlyCollection<Event> GetEventsById(uint eventAggregateId);

        bool SaveEvents(uint eventAggregateId, IReadOnlyCollection<Event> unsavedChanges);
    }
}
