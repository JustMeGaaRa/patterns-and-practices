using System.Collections;
using System.Collections.Generic;
using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore
{
    public class EventAggregate<TKey> : EntityBase<TKey>, IEnumerable<Event>
    {
        private readonly List<Event> _innerEvents = new List<Event>();

        protected void Apply<TEvent>(TEvent instance) where TEvent : Event
        {
            _innerEvents.Add(instance);
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return _innerEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}