using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore
    {
        ICollection<Event> GetEventsById(uint eventAggregateId);

        bool SaveEvents(uint eventAggregateId, ICollection<Event> unsavedChanges);
    }
}
