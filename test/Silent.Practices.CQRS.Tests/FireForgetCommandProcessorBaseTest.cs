using Silent.Practices.CQRS.Processors;
using Silent.Practices.CQRS.Tests.Fakes;
using System;
using Xunit;

namespace Silent.Practices.CQRS.Tests
{
    public class FireForgetCommandProcessorBaseTest
    {
        [Fact]
        public void CanProcess_WithRegisteredHandler_ShouldReturnTrue()
        {
            // Assign
            FakeCommand command = new FakeCommand();
            ICommandProcessor processor = new FakeFireForgetCommandProcessor();

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
            ICommandProcessor processor = new FakeFireForgetCommandProcessor();

            // Act
            bool canProcess = processor.CanProcess(command);

            // Assert
            Assert.False(canProcess);
        }

        [Fact]
        public void Process_WithNotRegisteredHandler_ShouldThrowException()
        {
            // Assign
            FakeUnregisteredCommand command = new FakeUnregisteredCommand();
            ICommandProcessor processor = new FakeFireForgetCommandProcessor();

            // Act, Assert
            Assert.Throws<ArgumentException>(() => processor.Process(command));
        }
    }
}
