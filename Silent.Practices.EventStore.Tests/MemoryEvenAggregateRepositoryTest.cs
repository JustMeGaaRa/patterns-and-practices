using System;
using System.Collections.Generic;
using Moq;
using Silent.Practices.EventStore.Tests.Fakes;
using Xunit;

namespace Silent.Practices.EventStore.Tests
{
    public class MemoryEvenAggregateRepositoryTest
    {
        [Fact]
        public void Ctor_NullObject_ShouldThrowException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(
                () => new MemoryEvenAggregateRepository<FakeEventAggregate>(null));
        }

        [Fact]
        public void Save_NullObject_ShouldThrowException()
        {
            // Arrange
            IEventStore eventStore = CreateDummyEventStore();
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => repository.Save(null));
        }

        [Fact]
        public void Save_EmptyFakeObject_ShouldBeIgnored()
        {
            // Arrange
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock();
            IEventStore eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = new FakeEventAggregate();
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            bool result = repository.Save(fakeEventAggregate);

            // Assert
            Assert.False(result);
            eventStoreMock.Verify(
                m => m.SaveEvents(
                    It.IsAny<uint>(),
                    It.IsAny<ICollection<Event>>()),
                Times.Never);
        }

        [Fact]
        public void Save_FakeObject_ShouldBeSaved()
        {
            // Arrange
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock();
            IEventStore eventStore = eventStoreMock.Object;
            FakeEventAggregate fakeEventAggregate = CreateDirtyFake(1);
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            bool result = repository.Save(fakeEventAggregate);

            // Assert
            Assert.True(result);
            eventStoreMock.Verify(
                m => m.SaveEvents(
                    It.IsAny<uint>(),
                    It.IsAny<ICollection<Event>>()),
                Times.Once);
        }

        [Fact]
        public void GetById_OnEmptyRepository_ShouldReturnNull()
        {
            // Arrange
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock();
            IEventStore eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

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
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock(eventAggregateId);
            IEventStore eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

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
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock();
            IEventStore eventStore = eventStoreMock.Object;
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

            // Act
            repository.GetById(1);

            // Assert
            eventStoreMock.Verify(
                m => m.GetEventsById(It.IsAny<uint>()),
                Times.Once);
        }

        private Mock<IEventStore> CreateEventStoreMock(uint eventAggregateId = 0)
        {
            var eventList = new List<Event>
            {
                new FakeCreatedEvent { AggregateId = eventAggregateId }
            };

            Mock<IEventStore> eventStoreMock = new Mock<IEventStore>();
            eventStoreMock.Setup(
                m => m.SaveEvents(
                    It.IsAny<uint>(),
                    It.IsAny<ICollection<Event>>()))
                .Returns(true);
            eventStoreMock.Setup(
                m => m.GetEventsById(
                    eventAggregateId))
                .Returns(eventList);

            return eventStoreMock;
        }

        private IEventStore CreateDummyEventStore()
        {
            Mock<IEventStore> eventStoreMock = CreateEventStoreMock();
            return eventStoreMock.Object;
        }

        private FakeEventAggregate CreateDirtyFake(uint aggregateId)
        {
            FakeEventAggregate fakeEventAggregate = new FakeEventAggregate(aggregateId, "Value");
            fakeEventAggregate.ChangeValue("New Value");
            return fakeEventAggregate;
        }
    }
}
