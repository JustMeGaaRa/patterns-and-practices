using Silent.Practices.EventStore;

namespace Silent.Practices.DDD.Tests.Fakes
{
    public abstract class Event : Event<uint>
    {
        protected Event(uint entityId) : base(entityId)
        {
        }
    }
}