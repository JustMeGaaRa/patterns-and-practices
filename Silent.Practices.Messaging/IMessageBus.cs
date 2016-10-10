using System.Collections.Generic;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public interface IMessageBus<in TMessage>
    {
        bool Register<TConcrete>(IHandler<TConcrete> handler) where TConcrete : TMessage;

        IReadOnlyCollection<IHandler<TConcrete>> GetHandlers<TConcrete>() where TConcrete : TMessage;

        void Send<TConcrete>(TConcrete message);
    }
}
