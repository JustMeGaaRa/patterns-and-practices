using System;
using System.Collections.Generic;
using System.Linq;
using Silent.Practices.Diagnostics;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public class MemoryMessageBus<TMessage> : IMessageBus<TMessage>
    {
        private readonly Dictionary<Type, List<object>> _messageHandlers;

        public MemoryMessageBus()
        {
            _messageHandlers = new Dictionary<Type, List<object>>();
        }

        public bool Register<TConcrete>(IHandler<TConcrete> handler) where TConcrete : TMessage
        {
            Contract.NotNull(handler, nameof(handler));

            Type messageType = typeof(TConcrete);
            EnsureTypeRegistratin(messageType);
            _messageHandlers[messageType].Add(handler);
            return true;
        }

        public IReadOnlyCollection<IHandler<TConcrete>> GetHandlers<TConcrete>() where TConcrete : TMessage
        {
            return InnerGetHandlers<TConcrete>();
        }

        public void Send<TConcrete>(TConcrete message)
        {
            Contract.NotNull(message, nameof(message));

            Type messageType = message.GetType();
            var handlers = InnerGetHandlers<TConcrete>();

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
                _messageHandlers.Add(type, new List<object>());
            }
        }

        private IReadOnlyCollection<IHandler<TConcrete>> InnerGetHandlers<TConcrete>()
        {
            return _messageHandlers.ContainsKey(typeof(TConcrete))
                ? _messageHandlers[typeof(TConcrete)].Cast<IHandler<TConcrete>>().ToList()
                : default(IReadOnlyCollection<IHandler<TConcrete>>);
        }
    }
}
