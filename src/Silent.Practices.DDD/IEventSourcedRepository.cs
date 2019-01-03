using Silent.Practices.Persistance;
using System;

namespace Silent.Practices.DDD
{
    public interface IEventSourcedRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : EventAggregate<TKey>
    {
    }

    public interface IEventSourcedRepositoryWithGuidKey<TEntity> : IEventSourcedRepository<TEntity, Guid>
        where TEntity : EventAggregate<Guid>
    {
    }

    public interface IEventSourcedRepositoryWithStringKey<TEntity> : IEventSourcedRepository<TEntity, string>
        where TEntity : EventAggregate<string>
    {
    }
}