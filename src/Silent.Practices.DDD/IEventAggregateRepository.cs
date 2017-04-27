using Silent.Practices.EventStore;
using Silent.Practices.Persistance;

namespace Silent.Practices.DDD
{
    public interface IEventAggregateRepository<TEntity, in TKey, TEventBase> : IRepository<TEntity, TKey> 
        where TEntity : EventAggregate<TKey, TEventBase>
    {
    }

    public interface IEventAggregateRepository<TEntity> : IRepository<TEntity>
        where TEntity : EventAggregate<uint, Event<uint>>
    {
    }
}