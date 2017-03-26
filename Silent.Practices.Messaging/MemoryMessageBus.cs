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
            EnsureTypeRegistration(messageType, handler);
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

            if(handlers != null)
            {
                foreach (var handler in handlers)
                {
                    handler.Handle(message);
                }
            }
        }

        private void EnsureTypeRegistration(Type type, object handler)
        {
            if (!_messageHandlers.ContainsKey(type))
            {
                _messageHandlers.Add(type, new List<object>());
            }

            _messageHandlers[type].Add(handler);
        }

        private IEnumerable<IHandler<TConcrete>> InnerGetHandlers<TConcrete>()
        {
            return _messageHandlers.ContainsKey(typeof(TConcrete))
                ? _messageHandlers[typeof(TConcrete)].Cast<IHandler<TConcrete>>()
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
