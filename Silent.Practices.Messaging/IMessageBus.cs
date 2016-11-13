using System.Collections.Generic;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public interface IMessageBus<in TMessage> :
        IPublisher<TMessage>,
        ISubscriber<TMessage>
    {
        IReadOnlyCollection<IHandler<TConcrete>> GetSubscriptions<TConcrete>() where TConcrete : TMessage;
    }
}
