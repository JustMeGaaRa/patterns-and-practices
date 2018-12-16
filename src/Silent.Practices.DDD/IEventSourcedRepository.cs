using Silent.Practices.Persistance;

namespace Silent.Practices.DDD
{
    public interface IEventSourcedRepository<TEntity> : IRepositoryWithGuidKey<TEntity> where TEntity : EventAggregate
    {
    }
}