using Silent.Practices.CQRS.Processors;
using Silent.Practices.CQRS.Tests.Fakes;
using System;
using Xunit;

namespace Silent.Practices.CQRS.Tests
{
    public class StatefulCommandProcessorBaseTest
    {
        [Fact]
        public void CanProcess_WithRegisteredHandler_ShouldReturnTrue()
        {
            // Assign
            FakeCommand command = new FakeCommand();
            ICommandProcessor<FakeStatus> processor = new FakeStatefulCommandProcessor();

            // Act
            bool canProcess = processor.CanProcess(command);

            // Assert
            Assert.True(canProcess);
        }

        [Fact]
        public void CanProcess_WithNotRegisteredHandler_ShouldReturnFalse()
        {
            // Assign
            FakeUnregisteredCommand command = new FakeUnregisteredCommand();
            ICommandProcessor<FakeStatus> processor = new FakeStatefulCommandProcessor();

            // Act
            bool canProcess = processor.CanProcess(command);

            // Assert
            Assert.False(canProcess);
        }

        [Fact]
        public void Process_WithRegisteredHandler_ShouldReturnNotNull()
        {
            // Assign
            FakeCommand command = new FakeCommand();
            ICommandProcessor<FakeStatus> processor = new FakeStatefulCommandProcessor();

            // Act
            FakeStatus   result = processor.Process(command);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Process_WithNotRegisteredHandler_ShouldThrowException()
        {
            // Assign
            FakeUnregisteredCommand command = new FakeUnregisteredCommand();
            ICommandProcessor<FakeStatus> processor = new FakeStatefulCommandProcessor();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => processor.Process(command));
        }
    }
}
