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
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventStore.SaveEvents(1, null));
        }

        [Fact]
        public void SaveEvents_WithEmptyArray_ShouldReturnTrue()
        {
            // Arrange
            IEvent[] fakeEvents = {};
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);

            // Act
            bool result = eventStore.SaveEvents(1, fakeEvents);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SaveEvents_WithNotEmptyArray_ShouldReturnTrue()
        {
            // Arrange
            IEvent[] fakeEvents = { new FakeEvent() };
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);

            // Act
            bool result = eventStore.SaveEvents(1, fakeEvents);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetEventsById_OnEmptyStore_ShouldReturnEmptyCollection()
        {
            // Arrange
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);

            // Act
            IReadOnlyCollection<IEvent> result = eventStore.GetEventsById(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetEventsById_WithUnexistingId_ShouldReturnEmptyCollection()
        {
            // Arrange
            uint eventAggregateId = 1;
            IEvent[] fakeEvents = { new FakeEvent() };
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);
            eventStore.SaveEvents(eventAggregateId, fakeEvents);

            // Act
            IReadOnlyCollection<IEvent> result = eventStore.GetEventsById(2);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetEventsById_WithExistingId_ShouldReturnEventCollection()
        {
            // Arrange
            uint eventAggregateId = 1;
            IEvent[] fakeEvents = { new FakeEvent() };
            IComparer<IEvent> comparer = CreateEventComparer();
            IEventStore<uint, IEvent> eventStore = new MemoryEventStore<uint, IEvent>(comparer);

            // Act
            eventStore.SaveEvents(eventAggregateId, fakeEvents);
            IEnumerable<IEvent> result = eventStore.GetEventsById(eventAggregateId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        private IComparer<IEvent> CreateEventComparer()
        {
            return Comparer<IEvent>.Create(CompareEvents);
        }

        private int CompareEvents(IEvent left, IEvent right)
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

        private class FakeEvent : Event<uint>
        {
        }
    }
}
