using System;
using System.Collections.Generic;
using Moq;
using Silent.Practices.DDD.Tests.Fakes;
using Silent.Practices.EventStore;
using Xunit;

namespace Silent.Practices.DDD.Tests
{
    public class MemoryEvenAggregateRepositoryTest
    {
        [Fact]
        public void Ctor_NullObject_ShouldThrowException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(
                () => new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(null));
        }

        [Fact]
        public void Add_NullObject_ShouldThrowException()
        {
            // Arrange
            IEventStore<uint, Event> eventStore = CreateDummyEventStore();
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => repository.Add(null));
        }

        [Fact]
        public void Add_EmptyFakeObject_ShouldBeIgnored()
        {
            // Arrange
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock();
            IEventStore<uint, Event> eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = new FakeEventAggregate();
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act
            bool result = repository.Add(fakeEventAggregate);

            // Assert
            Assert.False(result);
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<uint>(), It.IsAny<IReadOnlyCollection<Event>>()), Times.Never);
        }

        [Fact]
        public void Add_FakeObject_ShouldBeSaved()
        {
            // Arrange
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock();
            IEventStore<uint, Event> eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = CreateDirtyFake(1);
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act
            bool result = repository.Add(fakeEventAggregate);

            // Assert
            Assert.True(result);
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<uint>(), It.IsAny<IReadOnlyCollection<Event>>()), Times.Once);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldReturnNull()
        {
            // Arrange
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock();
            IEventStore<uint, Event> eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act
            var result = repository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetById_WithExistingid_ShouldReturnNotNull()
        {
            // Arrange
            uint eventAggregateId = 1;
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock(eventAggregateId);
            IEventStore<uint, Event> eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act
            var result = repository.GetById(eventAggregateId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventAggregateId, result.Id);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldCallEventStore()
        {
            // Arrange
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock();
            IEventStore<uint, Event> eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate, uint, Event> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate, uint, Event>(eventStore);

            // Act
            repository.GetById(1);

            // Assert
            eventStoreMock.Verify(m => m.GetEventsById(It.IsAny<uint>()), Times.Once);
        }

        private Mock<IEventStore<uint, Event>> CreateEventStoreMock(uint eventAggregateId = 0)
        {
            var eventList = new List<Event>
            {
                new FakeCreatedEvent(eventAggregateId)
            };

            Mock<IEventStore<uint, Event>> eventStoreMock = new Mock<IEventStore<uint, Event>>();
            eventStoreMock.Setup(m => m.SaveEvents(It.IsAny<uint>(), It.IsAny<IReadOnlyCollection<Event>>())).Returns(true);
            eventStoreMock.Setup(m => m.GetEventsById(eventAggregateId)).Returns(eventList);

            return eventStoreMock;
        }

        private IEventStore<uint, Event> CreateDummyEventStore()
        {
            Mock<IEventStore<uint, Event>> eventStoreMock = CreateEventStoreMock();
            return eventStoreMock.Object;
        }

        private FakeEventAggregate CreateDirtyFake(uint aggregateId)
        {
            return new FakeEventAggregate(aggregateId);
        }
    }
}
