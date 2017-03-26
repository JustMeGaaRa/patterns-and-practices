using System;
using Moq;
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
            Mock<Action<DummyMessage>> handlerMock = new Mock<Action<DummyMessage>>();
            Action<DummyMessage> fakeMessageHandler = handlerMock.Object;

            // Act
            messageBus.Subscribe(fakeMessageHandler);
            messageBus.Publish(new DummyMessage());

            // Assert
            handlerMock.Verify(m => m(It.IsNotNull<DummyMessage>()), Times.Once);
        }
    }
}