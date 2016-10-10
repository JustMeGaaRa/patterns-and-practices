using Xunit;

namespace Silent.Practices.EventStore.Tests
{
    public class MemoryEvenAggregateRepositoryTest
    {
        [Fact]
        public void TestMethod1()
        {
            // Arrange
            IEventStore eventStore = new MemoryEventStore();
            IEventAggregateRepository<FakeEventAggregate> repository =
                new MemoryEvenAggregateRepository<FakeEventAggregate>(eventStore);

            // Act

            // Assert

        }

        public class FakeEventAggregate : EventAggregate<int>
        {
            public FakeEventAggregate()
            {
            }

            public FakeEventAggregate(string value)
            {
                Apply(new FakeCreatedEvent { Value = value });
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

        private class FakeCreatedEvent : Event
        {
            public int AggregateId { get; set; }

            public string Value { get; set; }
        }

        private class FakeValueUpdatedEvent : Event
        {
            public int AggregateId { get; set; }

            public string NewValue { get; set; }
        }

        private class FakeDeletedEvent : Event
        {
            public int AggregateId { get; set; }
        }
    }
}
