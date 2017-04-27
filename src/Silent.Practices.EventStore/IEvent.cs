using System;

namespace Silent.Practices.EventStore
{
    public interface IEvent
    {
        DateTime Timestamp { get; }
    }
}