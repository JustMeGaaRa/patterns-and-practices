using System;
using System.Collections.Generic;
using Silent.Practices.DDD.Tests.Fakes;
using Xunit;

namespace Silent.Practices.DDD.Tests
{
    public class EventAggregateTest
    {
        [Fact]
        public void GetUncommitted_OnEmptyAggregate_ShouldReturnEmptyCollection()
        {
            // Arrange
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act
            IEnumerable<Event<Guid>> result = eventAggregate.GetUncommitted();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetUncommitted_OnCreatedAggregate_ShouldReturnCollection()
        {
            // Arrange
            EventAggregate eventAggregate = new FakeEventAggregate(Guid.NewGuid());

            // Act
            IEnumerable<Event<Guid>> result = eventAggregate.GetUncommitted();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void MarkAsCommitted_OnEmptyAggregate_ShouldNotContainUncommitted()
        {
            // Arrange
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act
            eventAggregate.MarkAsCommitted();
            IEnumerable<Event<Guid>> uncommitted = eventAggregate.GetUncommitted();

            // Assert
            Assert.Empty(uncommitted);
        }

        [Fact]
        public void MarkAsCommitted_OnCreatedAggregate_ShouldNotContainUncommitted()
        {
            // Arrange
            EventAggregate eventAggregate = new FakeEventAggregate(Guid.NewGuid());

            // Act
            eventAggregate.MarkAsCommitted();
            IEnumerable<Event<Guid>> uncommitted = eventAggregate.GetUncommitted();

            // Assert
            Assert.Empty(uncommitted);
        }

        [Fact]
        public void ApplyHistory_NullCollection_ShouldThrowException()
        {
            // Arrange
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventAggregate.ApplyHistory(null));
        }

        [Fact]
        public void ApplyHistory_WithNotEmptyEventHistory_ShouldApplyChanges()
        {
            // Arrange
            Guid eventAggregateId = Guid.NewGuid();
            EventWithGuidKey createdEvent = new FakeCreatedEvent(eventAggregateId, 1);
            IEnumerable<EventWithGuidKey> historyEvents = new [] { createdEvent };
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act
            Guid beforeChangesId = eventAggregate.EntityId;
            eventAggregate.ApplyHistory(historyEvents);

            // Assert
            Assert.NotEqual(eventAggregateId, beforeChangesId);
            Assert.Equal(eventAggregateId, eventAggregate.EntityId);
        }

        [Fact]
        public void ApplyHistory_WithNullObject_ShouldThrowException()
        {
            // Arrange
            IEnumerable<EventWithGuidKey> historyEvents = new EventWithGuidKey[] { null };
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => eventAggregate.ApplyHistory(historyEvents));
        }

        [Fact]
        public void ApplyHistory_WithNotSupportedEventType_ShouldThrowException()
        {
            // Arrange
            EventWithGuidKey deletedEvent = new FakeDeletedEvent(Guid.NewGuid(), 1);
            IEnumerable<EventWithGuidKey> historyEvents = new[] { deletedEvent };
            EventAggregate eventAggregate = new FakeEventAggregate();

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => eventAggregate.ApplyHistory(historyEvents));
        }
    }
}
