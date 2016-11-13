using System;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public static class MessageBusExtension
    {
        public static IDisposable Subscribe<TMessage, TConcrete>(
            this ISubscriber<TMessage> subscriber,
            Action<TConcrete> messageHandlerFunc)
            where TConcrete : TMessage
        {
            return subscriber.Subscribe(new AnonymousHandler<TConcrete>(messageHandlerFunc));
        }

        private class AnonymousHandler<TMessage> : IHandler<TMessage>
        {
            private readonly Action<TMessage> _messageHandlerFunc;

            public AnonymousHandler(Action<TMessage> messageHandlerFunc)
            {
                if (messageHandlerFunc == null)
                {
                    throw new ArgumentNullException(nameof(messageHandlerFunc));
                }

                _messageHandlerFunc = messageHandlerFunc;
            }

            public void Handle(TMessage instance)
            {
                _messageHandlerFunc(instance);
            }
        }
    }
}
