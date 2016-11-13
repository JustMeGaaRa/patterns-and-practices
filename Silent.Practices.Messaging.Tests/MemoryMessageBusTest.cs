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
            IHandler<DummyMessage> fakeMessageHandler = Mock.Of<IHandler<DummyMessage>>();

            // Act
            messageBus.Subscribe(fakeMessageHandler);
            var handlers = messageBus.GetSubscriptions<DummyMessage>();

            // Assert
            Assert.NotNull(handlers);
            Assert.NotEmpty(handlers);
        }

        [Fact]
        public void Subscribe_WhenUsingDisposable_ShouldUnsubscribe()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            IHandler<DummyMessage> fakeMessageHandler = Mock.Of<IHandler<DummyMessage>>();

            // Act
            IDisposable unsubsribable = messageBus.Subscribe(fakeMessageHandler);
            unsubsribable.Dispose();
            var handlers = messageBus.GetSubscriptions<DummyMessage>();

            // Assert
            Assert.Null(handlers);
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
        public void Publish_WithoutHandlerButFakeMessage_ShouldThrowException()
        {
            // Arrange
            IMessageBus<DummyMessage> messageBus = new MemoryMessageBus<DummyMessage>();
            DummyMessage dummyMessage = new DummyMessage();

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => messageBus.Publish(dummyMessage));
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
