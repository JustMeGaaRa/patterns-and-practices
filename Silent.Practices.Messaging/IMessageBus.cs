using System.Collections.Generic;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public interface IMessageBus<TMessage>
    {
        bool Register<TConcrete>(IHandler<TMessage> handler) where TConcrete : TMessage;

        IReadOnlyCollection<IHandler<TMessage>> GetHandlers<TConcrete>() where TConcrete : TMessage;

        void Send(TMessage message);
    }
}
