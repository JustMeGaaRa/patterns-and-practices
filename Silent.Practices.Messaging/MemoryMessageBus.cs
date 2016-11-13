using System;
using System.Collections.Generic;
using System.Linq;
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

        public IDisposable Subscribe<TConcrete>(IHandler<TConcrete> handler) where TConcrete : TMessage
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Type messageType = typeof(TConcrete);
            EnsureTypeRegistratin(messageType);
            _messageHandlers[messageType].Add(handler);
            return new UnsubscribeDisposable<TConcrete>(_messageHandlers);
        }

        public void Publish<TConcrete>(TConcrete message) where TConcrete : TMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

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

        public IReadOnlyCollection<IHandler<TConcrete>> GetSubscriptions<TConcrete>() where TConcrete : TMessage
        {
            return InnerGetHandlers<TConcrete>();
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

        private class UnsubscribeDisposable<TConcrete> : IDisposable
        {
            private readonly Dictionary<Type, List<object>> _messageHandlers;

            public UnsubscribeDisposable(Dictionary<Type, List<object>> messageHandlers)
            {
                _messageHandlers = messageHandlers;
            }

            public void Dispose()
            {
                _messageHandlers.Remove(typeof (TConcrete));
            }
        }
    }
}
