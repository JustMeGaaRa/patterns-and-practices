using System;
using System.Collections.Generic;

namespace Silent.Practices.EventStore
{
    public interface IEventStore<in TKey, TEvent>
    {
        IReadOnlyCollection<TEvent> GetEventsById(TKey eventAggregateId);

        IReadOnlyCollection<TEvent> GetEvents(Func<TEvent, bool> filter = null);

        bool SaveEvents(TKey eventAggregateId, IReadOnlyCollection<TEvent> unsavedChanges);
    }
}
