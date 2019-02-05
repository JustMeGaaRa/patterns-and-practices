using System;
using System.Collections.Generic;
using System.Threading;

namespace Silent.Practices.DDD
{
    public abstract class EventAggregate<TKey> : Entity<TKey>, IArchivable
    {
        private readonly List<Event<TKey>> _uncommittedChanges = new List<Event<TKey>>();
        private readonly Dictionary<Type, Action<Event<TKey>>> _eventHandlers = new Dictionary<Type, Action<Event<TKey>>>();
        private long _currentAggregateVersion = 0;

        public EventAggregate()
        {
            RegisterEventHandler<EventAggregateArchivedEvent<TKey>>(x => InternalSetArchived());
        }

        public long Version
        {
            get { return _currentAggregateVersion; }
            protected set { _currentAggregateVersion = value; }
        }

        public bool IsArchived { get; protected set; }

        public void Archive()
        {
            InternalSetArchived();
            AddUncommitedEvent(new EventAggregateArchivedEvent<TKey>(EntityId, GetNextAggregateVersion()));
        }

        public IReadOnlyCollection<Event<TKey>> GetUncommitted() => _uncommittedChanges;

        public void MarkAsCommitted() => _uncommittedChanges.Clear();

        public void ApplyHistory(IEnumerable<Event<TKey>> historicalEvents)
        {
            if (historicalEvents == null)
            {
                throw new ArgumentNullException(nameof(historicalEvents));
            }

            foreach (Event<TKey> historyEvent in historicalEvents)
            {
                // NOTE: propagates generic TEvent type as base type
                ApplyEvent(historyEvent, false);
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler) where TEvent : Event<TKey>
        {
            Action<Event<TKey>> genericHandler = eventInstance => handler.Invoke((TEvent)eventInstance);
            _eventHandlers[typeof(TEvent)] = genericHandler;
        }

        protected long GetNextAggregateVersion() => Interlocked.Increment(ref _currentAggregateVersion);

        protected long RestoreAggregateVersion(long version) => Interlocked.Exchange(ref _currentAggregateVersion, version);

        protected void AddUncommitedEvent<TEvent>(TEvent instance) where TEvent : Event<TKey> => _uncommittedChanges.Add(instance);

        protected void ApplyEvent<TEvent>(TEvent instance) where TEvent : Event<TKey> => ApplyEvent(instance, true);

        private void ApplyEvent<TEvent>(TEvent instance, bool isNew) where TEvent : Event<TKey>
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "Event instance cannot be null.");
            }

            if (isNew)
            {
                _uncommittedChanges.Add(instance);
            }

            // NOTE: instance.GetType() should not be changed to typeof(TEvent)
            // typeof(TEvent) returns base instance type
            // instance.GetType() returns actual instance type (is polymorphic)
            if (!_eventHandlers.ContainsKey(instance.GetType()))
            {
                throw new NotSupportedException($"Event of type '{typeof(TEvent)}' cannot be handled because no handler was registered.");
            }

            RestoreAggregateVersion(instance.Version);
            _eventHandlers[instance.GetType()].Invoke(instance);
        }

        private void InternalSetArchived() => IsArchived = true;
    }

    public abstract class EventAggregate : EventAggregate<Guid>
    {
        protected EventAggregate() => EntityId = Guid.NewGuid();
    }
}