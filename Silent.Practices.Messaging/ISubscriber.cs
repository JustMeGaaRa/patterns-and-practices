using System;
using Silent.Practices.Patterns;

namespace Silent.Practices.Messaging
{
    public interface ISubscriber<in TMessage>
    {
        IDisposable Subscribe<TConcrete>(IHandler<TConcrete> handler) where TConcrete : TMessage;
    }
}