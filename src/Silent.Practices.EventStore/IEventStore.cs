using System;
using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore
    {
        IReadOnlyCollection<Event> GetEventsById(uint eventAggregateId);

        IReadOnlyCollection<Event> GetEvents(Func<Event, bool> filter = null);

        bool SaveEvents(uint eventAggregateId, IReadOnlyCollection<Event> unsavedChanges);
    }
}
