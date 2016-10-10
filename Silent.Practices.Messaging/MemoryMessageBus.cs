using System;
using System.Collections.Generic;
using Silent.Practices.Diagnostics;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public class MemoryMessageBus<TMessage> : IMessageBus<TMessage>
    {
        private readonly Dictionary<Type, List<IHandler<TMessage>>> _messageHandlers;

        public MemoryMessageBus()
        {
            _messageHandlers = new Dictionary<Type, List<IHandler<TMessage>>>();
        } 

        public bool Register<TConcrete>(IHandler<TMessage> handler) where TConcrete : TMessage
        {
            Contract.NotNull(handler, nameof(handler));

            Type messageType = typeof(TConcrete);
            EnsureTypeRegistratin(messageType);
            _messageHandlers[messageType].Add(handler);
            return true;
        }

        public IReadOnlyCollection<IHandler<TMessage>> GetHandlers<TConcrete>() where TConcrete : TMessage
        {
            Type messageType = typeof(TConcrete);
            return InnerGetHandlers(messageType);
        }

        public void Send(TMessage message)
        {
            Contract.NotNull(message, nameof(message));

            Type messageType = message.GetType();
            var handlers = InnerGetHandlers(messageType);

            if (handlers == null)
            {
                throw new NotSupportedException($"No handlers of type {messageType.Name} were registered");
            }

            foreach (var handler in handlers)
            {
                handler.Handle(message);
            }
        }

        private void EnsureTypeRegistratin(Type type)
        {
            if (!_messageHandlers.ContainsKey(type))
            {
                _messageHandlers.Add(type, new List<IHandler<TMessage>>());
            }
        }

        private IReadOnlyCollection<IHandler<TMessage>> InnerGetHandlers(Type messageType)
        {
            return _messageHandlers.ContainsKey(messageType)
                ? _messageHandlers[messageType].AsReadOnly()
                : default(IReadOnlyCollection<IHandler<TMessage>>);
        }
    }
}
