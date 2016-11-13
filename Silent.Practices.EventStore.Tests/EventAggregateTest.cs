using System;
using System.Collections.Generic;
using Silent.Practices.EventStore.Tests.Fakes;
using Xunit;

namespace Silent.Practices.EventStore.Tests
{
    public class EventAggregateTest
    {
        [Fact]
        public void GetUncommitted_OnEmptyAggregate_ShouldReturnEmptyCollection()
        {
            // Arrange
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act
            IEnumerable<Event> result = eventAggregate.GetUncommitted();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetUncommitted_OnCreatedAggregate_ShouldReturnCollection()
        {
            // Arrange
            EventAggregate<uint> eventAggregate = new FakeEventAggregate(1);

            // Act
            IEnumerable<Event> result = eventAggregate.GetUncommitted();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void MarkAsCommitted_OnEmptyAggregate_ShouldNotContainUncommitted()
        {
            // Arrange
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act
            eventAggregate.MarkAsCommitted();
            IEnumerable<Event> uncommitted = eventAggregate.GetUncommitted();

            // Assert
            Assert.Empty(uncommitted);
        }

        [Fact]
        public void MarkAsCommitted_OnCreatedAggregate_ShouldNotContainUncommitted()
        {
            // Arrange
            EventAggregate<uint> eventAggregate = new FakeEventAggregate(1);

            // Act
            eventAggregate.MarkAsCommitted();
            IEnumerable<Event> uncommitted = eventAggregate.GetUncommitted();

            // Assert
            Assert.Empty(uncommitted);
        }

        [Fact]
        public void ApplyHistory_NullCollection_ShouldThrowException()
        {
            // Arrange
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventAggregate.ApplyHistory(null));
        }

        [Fact]
        public void ApplyHistory_WithNotEmptyEventHistory_ShouldApplyChanges()
        {
            // Arrange
            uint eventAggregateId = 1;
            Event createdEvent = new FakeCreatedEvent(eventAggregateId) { Value = "New Value" };
            IEnumerable<Event> historyEvents = new [] { createdEvent };
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act
            uint beforeChangesId = eventAggregate.Id;
            eventAggregate.ApplyHistory(historyEvents);

            // Assert
            Assert.NotEqual(eventAggregateId, beforeChangesId);
            Assert.Equal(eventAggregateId, eventAggregate.Id);
        }

        [Fact]
        public void ApplyHistory_WithNullObject_ShouldThrowException()
        {
            // Arrange
            IEnumerable<Event> historyEvents = new Event[] { null };
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventAggregate.ApplyHistory(historyEvents));
        }

        [Fact]
        public void ApplyHistory_WithNotSupportedEventType_ShouldThrowException()
        {
            // Arrange
            Event deletedEvent = new FakeDeletedEvent(1);
            IEnumerable<Event> historyEvents = new[] { deletedEvent };
            EventAggregate<uint> eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => eventAggregate.ApplyHistory(historyEvents));
        }
    }
}
