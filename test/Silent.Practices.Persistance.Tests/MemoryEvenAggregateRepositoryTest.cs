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
                () => new MemoryEventAggregateRepository<FakeEventAggregate>(null));
        }

        [Fact]
        public void Add_NullObject_ShouldThrowException()
        {
            // Arrange
            IEventStore<Guid, EventWithGuidKey> eventStore = CreateDummyEventStore();
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => repository.SaveAsync(null));
        }

        [Fact]
        public async void Add_EmptyFakeObject_ShouldBeIgnored()
        {
            // Arrange
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock();
            IEventStore<Guid, EventWithGuidKey> eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = new FakeEventAggregate();
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            bool result = await repository.SaveAsync(fakeEventAggregate);

            // Assert
            Assert.False(result);
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<Guid>(), It.IsAny<IReadOnlyCollection<EventWithGuidKey>>()), Times.Never);
        }

        [Fact]
        public async void Add_FakeObject_ShouldBeSaved()
        {
            // Arrange
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock();
            IEventStore<Guid, EventWithGuidKey> eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = CreateDirtyFake(Guid.NewGuid());
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            bool result = await repository.SaveAsync(fakeEventAggregate);

            // Assert
            Assert.True(result);
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<Guid>(), It.IsAny<IReadOnlyCollection<EventWithGuidKey>>()), Times.Once);
        }

        [Fact]
        public async void GetById_OnEmptyRepository_ShouldReturnNull()
        {
            // Arrange
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock();
            IEventStore<Guid, EventWithGuidKey> eventStore = eventStoreMock.Object;
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            FakeEventAggregate result = await repository.FindByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetById_WithExistingid_ShouldReturnNotNull()
        {
            // Arrange
            Guid eventAggregateId = Guid.NewGuid();
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock(eventAggregateId);
            IEventStore<Guid, EventWithGuidKey> eventStore = eventStoreMock.Object;
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            FakeEventAggregate result = await repository.FindByIdAsync(eventAggregateId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventAggregateId, result.EntityId);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldCallEventStore()
        {
            // Arrange
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock();
            IEventStore<Guid, EventWithGuidKey> eventStore = eventStoreMock.Object;
            IEventSourcedRepository<FakeEventAggregate> repository =
                new MemoryEventAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            repository.FindByIdAsync(Guid.NewGuid());

            // Assert
            eventStoreMock.Verify(m => m.GetEventsById(It.IsAny<Guid>()), Times.Once);
        }

        private Mock<IEventStore<Guid, EventWithGuidKey>> CreateEventStoreMock(Guid eventAggregateId = default(Guid))
        {
            var eventList = new List<EventWithGuidKey>
            {
                new FakeCreatedEvent(eventAggregateId)
            };

            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = new Mock<IEventStore<Guid, EventWithGuidKey>>();
            eventStoreMock.Setup(m => m.SaveEvents(It.IsAny<Guid>(), It.IsAny<IReadOnlyCollection<EventWithGuidKey>>())).Returns(true);
            eventStoreMock.Setup(m => m.GetEventsById(eventAggregateId)).Returns(eventList);

            return eventStoreMock;
        }

        private IEventStore<Guid, EventWithGuidKey> CreateDummyEventStore()
        {
            Mock<IEventStore<Guid, EventWithGuidKey>> eventStoreMock = CreateEventStoreMock();
            return eventStoreMock.Object;
        }

        private FakeEventAggregate CreateDirtyFake(Guid aggregateId)
        {
            return new FakeEventAggregate(aggregateId);
        }

        internal sealed class FakeEventAggregate : EventAggregate
        {
            public FakeEventAggregate()
            {
                RegisterEventHandler<FakeCreatedEvent>(x => EntityId = x.EntityId);
                RegisterEventHandler<FakeValueUpdatedEvent>(x => Value = x.NewValue);
            }

            public FakeEventAggregate(Guid eventAggregateId) : this()
            {
                ApplyEvent(new FakeCreatedEvent(eventAggregateId));
            }

            public string Value { get; private set; }

            public void SetValue(string newValue)
            {
                ApplyEvent(new FakeValueUpdatedEvent(EntityId) { NewValue = newValue });
            }
        }
    }
}
