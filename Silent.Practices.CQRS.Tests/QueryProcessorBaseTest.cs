using Silent.Practices.CQRS.Processors;
using Silent.Practices.CQRS.Tests.Fakes;
using System;
using Xunit;

namespace Silent.Practices.CQRS.Tests
{
    public class QueryProcessorBaseTest
    {
        [Fact]
        public void CanProcess_WithRegisteredHandler_ShouldReturnTrue()
        {
            // Assign
            FakeQuery query = new FakeQuery();
            IQueryProcessor processor = new FakeQueryProcessor();

            // Act
            bool canProcess = processor.CanProcess(query);

            // Assert
            Assert.True(canProcess);
        }

        [Fact]
        public void CanProcess_WithNotRegisteredHandler_ShouldReturnFalse()
        {
            // Assign
            FakeUnregisteredQuery query = new FakeUnregisteredQuery();
            IQueryProcessor processor = new FakeQueryProcessor();

            // Act
            bool canProcess = processor.CanProcess(query);

            // Assert
            Assert.False(canProcess);
        }

        [Fact]
        public void Process_WithRegisteredHandler_ShouldReturnNotNull()
        {
            // Assign
            FakeQuery query = new FakeQuery();
            IQueryProcessor processor = new FakeQueryProcessor();

            // Act
            FakeResult result = processor.Process(query);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Process_WithNotRegisteredHandler_ShouldThrowException()
        {
            // Assign
            FakeUnregisteredQuery query = new FakeUnregisteredQuery();
            IQueryProcessor processor = new FakeQueryProcessor();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => processor.Process(query));
        }
    }
}
