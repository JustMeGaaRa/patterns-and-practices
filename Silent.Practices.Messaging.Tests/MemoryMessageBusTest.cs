using System;
using Moq;
using Silent.Practices.Patterns;
using Xunit;

namespace Silent.Practices.Messaging.Tests
{
    public class MemoryMessageBusTest
    {
        [Fact]
        public void Register_NullObject_ShouldThrowException()
        {
            // Arrange
            IMessageBus<FakeMessage> messageBus = new MemoryMessageBus<FakeMessage>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => messageBus.Register<FakeMessage>(null));
        }

        [Fact]
        public void Register_OneHandler_ShouldContainOneHandler()
        {
            // Arrange
            IMessageBus<FakeMessage> messageBus = new MemoryMessageBus<FakeMessage>();
            IHandler<FakeMessage> fakeMessageHandler = Mock.Of<IHandler<FakeMessage>>();

            // Act
            messageBus.Register<FakeMessage>(fakeMessageHandler);
            var handlers = messageBus.GetHandlers<FakeMessage>();

            // Assert
            Assert.NotNull(handlers);
            Assert.NotEmpty(handlers);
        }

        [Fact]
        public void Send_WithHandlerAndNullObject_ShouldThrowException()
        {
            // Arrange
            IMessageBus<FakeMessage> messageBus = new MemoryMessageBus<FakeMessage>();
            IHandler<FakeMessage> fakeMessageHandler = Mock.Of<IHandler<FakeMessage>>();

            // Act
            messageBus.Register<FakeMessage>(fakeMessageHandler);

            // Assert
            Assert.Throws<ArgumentNullException>(() => messageBus.Send(null));
        }

        [Fact]
        public void Send_WithoutHandlerButFakeMessage_ShouldThrowException()
        {
            // Arrange
            IMessageBus<FakeMessage> messageBus = new MemoryMessageBus<FakeMessage>();
            FakeMessage fakeMessage = new FakeMessage();

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => messageBus.Send(fakeMessage));
        }

        [Fact]
        public void Send_FakeMessage_ShouldInvokeOneHandler()
        {
            // Arrange
            IMessageBus<FakeMessage> messageBus = new MemoryMessageBus<FakeMessage>();
            Mock<IHandler<FakeMessage>> mock = new Mock<IHandler<FakeMessage>>();
            mock.Setup(x => x.Handle(It.IsNotNull<FakeMessage>()));
            IHandler<FakeMessage> fakeMessageHandler = mock.Object;
            FakeMessage fakeMessage = new FakeMessage();

            // Act
            messageBus.Register<FakeMessage>(fakeMessageHandler);
            messageBus.Send(fakeMessage);

            // Assert
            mock.Verify(m => m.Handle(It.IsNotNull<FakeMessage>()), Times.Once);
        }

        public class FakeMessage
        {
        }
    }
}
