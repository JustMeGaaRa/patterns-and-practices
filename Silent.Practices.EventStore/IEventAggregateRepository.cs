using Silent.Practices.Persistance;

namespace Silent.Practices.EventStore
{
    public interface IEventAggregateRepository<TEntity> :
        IRepository<TEntity> where TEntity : EventAggregate<int>
    {
    }
}