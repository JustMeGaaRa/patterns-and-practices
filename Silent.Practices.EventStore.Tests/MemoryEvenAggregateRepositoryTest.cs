using System;
using System.Collections.Generic;
using Moq;
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
                    It.IsAny<IEnumerable<Event>>()),
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
                    It.IsAny<IEnumerable<Event>>()),
                Times.Once);
        }

        private Mock<IEventStore> CreateEventStoreMock()
        {
            Mock<IEventStore> eventStoreMock = new Mock<IEventStore>();
            eventStoreMock.Setup(
                m => m.SaveEvents(
                    It.IsAny<uint>(),
                    It.IsAny<IEnumerable<Event>>()))
                .Returns(true);

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

        public class FakeEventAggregate : EventAggregate<uint>
        {
            public FakeEventAggregate()
            {
                RegisterHandler<FakeCreatedEvent>(x => { });
                RegisterHandler<FakeValueUpdatedEvent>(x => { });
                RegisterHandler<FakeDeletedEvent>(x => { });
            }

            public FakeEventAggregate(uint eventAggregateId, string value) : this()
            {
                Apply(new FakeCreatedEvent { AggregateId = eventAggregateId, Value = value });
            }

            public void ChangeValue(string newValue)
            {
                Apply(new FakeValueUpdatedEvent { AggregateId = Id, NewValue = newValue });
            }

            public void Delete()
            {
                Apply(new FakeDeletedEvent { AggregateId = Id });
            }
        }

        public class FakeCreatedEvent : Event
        {
            public uint AggregateId { get; set; }

            public string Value { get; set; }
        }

        public class FakeValueUpdatedEvent : Event
        {
            public uint AggregateId { get; set; }

            public string NewValue { get; set; }
        }

        public class FakeDeletedEvent : Event
        {
            public uint AggregateId { get; set; }
        }
    }
}
