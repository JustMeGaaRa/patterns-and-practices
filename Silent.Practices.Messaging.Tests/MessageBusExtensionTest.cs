using System;
using Xunit;

namespace Silent.Practices.Messaging.Tests
{
    public class MessageBusExtensionTest
    {
        [Fact]
        public void Subscribe_NullDelegate_ShouldThrowException()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => messageBus.Subscribe((Action<DummyMessage>)null));
        }

        [Fact]
        public void Subscribe_OneDelegate_ShouldContainOneHandler()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            Action<DummyMessage> messageHandler = x => { /* simply do nothing */ };

            // Act
            messageBus.Subscribe(messageHandler);
            var handlers = messageBus.GetSubscriptions<DummyMessage>();

            // Assert
            Assert.NotNull(handlers);
            Assert.NotEmpty(handlers);
        }
    }
}