using Silent.Practices.Patterns;

namespace Silent.Practices.EventStore
{
    public interface IEventHandler<in TEvent> : IHandler<TEvent> where TEvent : Event
    {
    }
}