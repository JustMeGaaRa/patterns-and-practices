using System;
using Moq;
using Silent.Practices.Patterns;
using Xunit;

namespace Silent.Practices.Messaging.Tests
{
    public class MemoryMessageBusTest
    {
        [Fact]
        public void Subscribe_NullObject_ShouldThrowException()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => messageBus.Subscribe<DummyMessage>(null));
        }

        [Fact]
        public void Subscribe_OneHandler_ShouldContainOneHandler()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            Mock<IHandler<DummyMessage>> handlerMock = new Mock<IHandler<DummyMessage>>();
            IHandler<DummyMessage> fakeMessageHandler = handlerMock.Object;

            // Act
            messageBus.Subscribe(fakeMessageHandler);
            messageBus.Publish(new DummyMessage());

            // Assert
            handlerMock.Verify(m => m.Handle(It.IsNotNull<DummyMessage>()), Times.Once);
        }

        [Fact]
        public void Subscribe_WhenUsingDisposable_ShouldUnsubscribe()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            Mock<IHandler<DummyMessage>> handlerMock = new Mock<IHandler<DummyMessage>>();
            IHandler<DummyMessage> fakeMessageHandler = handlerMock.Object;

            // Act
            using (IDisposable unsubscribable = messageBus.Subscribe(fakeMessageHandler));
            messageBus.Publish(new DummyMessage());

            // Assert
            handlerMock.Verify(m => m.Handle(It.IsNotNull<DummyMessage>()), Times.Never);
        }

        [Fact]
        public void Publish_WithHandlerAndNullObject_ShouldThrowException()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            IHandler<DummyMessage> fakeMessageHandler = Mock.Of<IHandler<DummyMessage>>();

            // Act
            messageBus.Subscribe(fakeMessageHandler);

            // Assert
            Assert.Throws<ArgumentNullException>(() => messageBus.Publish<DummyMessage>(null));
        }

        [Fact]
        public void Publish_FakeMessage_ShouldInvokeOneHandler()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            Mock<IHandler<DummyMessage>> mock = new Mock<IHandler<DummyMessage>>();
            mock.Setup(x => x.Handle(It.IsNotNull<DummyMessage>()));
            IHandler<DummyMessage> fakeMessageHandler = mock.Object;
            DummyMessage dummyMessage = new DummyMessage();

            // Act
            messageBus.Subscribe(fakeMessageHandler);
            messageBus.Publish(dummyMessage);

            // Assert
            mock.Verify(m => m.Handle(It.IsNotNull<DummyMessage>()), Times.Once);
        }
    }
}
