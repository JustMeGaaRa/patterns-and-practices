using System;
using System.Collections.Generic;
using Xunit;

namespace Silent.Practices.EventStore.Tests
{
    public class MemoryEventStoreTest
    {
        [Fact]
        public void SaveEvents_NullObject_ShouldThrowException()
        {
            // Arrange
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventStore.SaveEvents(1, null));
        }

        [Fact]
        public void SaveEvents_WithEmptyArray_ShouldReturnTrue()
        {
            // Arrange
            Event[] fakeEvents = {};
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);

            // Act
            bool result = eventStore.SaveEvents(1, fakeEvents);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SaveEvents_WithNotEmptyArray_ShouldReturnTrue()
        {
            // Arrange
            Event[] fakeEvents = { new FakeEvent() };
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);

            // Act
            bool result = eventStore.SaveEvents(1, fakeEvents);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetEventsById_OnEmptyStore_ShouldReturnEmptyCollection()
        {
            // Arrange
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);

            // Act
            IReadOnlyCollection<Event> result = eventStore.GetEventsById(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetEventsById_WithUnexistingId_ShouldReturnEmptyCollection()
        {
            // Arrange
            uint eventAggregateId = 1;
            Event[] fakeEvents = { new FakeEvent() };
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);
            eventStore.SaveEvents(eventAggregateId, fakeEvents);

            // Act
            IReadOnlyCollection<Event> result = eventStore.GetEventsById(2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetEventsById_WithExistingId_ShouldReturnEventCollection()
        {
            // Arrange
            uint eventAggregateId = 1;
            Event[] fakeEvents = { new FakeEvent() };
            IComparer<Event> comparer = CreateEventComparer();
            IEventStore<uint, Event> eventStore = new MemoryEventStore<uint, Event>(comparer);

            // Act
            eventStore.SaveEvents(eventAggregateId, fakeEvents);
            IEnumerable<Event> result = eventStore.GetEventsById(eventAggregateId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        private IComparer<Event> CreateEventComparer()
        {
            return Comparer<Event>.Create(CompareEvents);
        }

        private int CompareEvents(Event left, Event right)
        {
            if (left.Timestamp > right.Timestamp)
            {
                return -1;
            }
            if (left.Timestamp < right.Timestamp)
            {
                return 1;
            }
            return 0;
        }

        private class Event
        {
            public Event()
            {
                Timestamp = DateTime.Now;
            }

            public DateTime Timestamp { get; set; }
        }

        private class FakeEvent : Event
        {
        }
    }
}
