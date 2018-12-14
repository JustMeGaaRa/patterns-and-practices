using Silent.Practices.DDD;
using System;
using System.Collections.Generic;

namespace Silent.Practices.Persistance
{
    public sealed class MemoryUnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories;

        public MemoryUnitOfWork()
        {
            _repositories = new Dictionary<Type, object>();
        }

        public void UseRepository<TEntity>(IRepositoryWithGuidKey<TEntity> repository) where TEntity : EntityWithGuidKey
        {
            _repositories[typeof(TEntity)] = repository;
        }

        public IRepositoryWithGuidKey<TEntity> GetRepository<TEntity>() where TEntity : EventWithGuidKey
        {
            Type entityType = typeof(TEntity);
            IRepositoryWithGuidKey<TEntity> repository = _repositories.ContainsKey(entityType)
                ? _repositories[entityType] as IRepositoryWithGuidKey<TEntity>
                : null;
            return repository;
        }

        public bool Commit()
        {
            throw new NotImplementedException();
        }

        public bool Rollback()
        {
            throw new NotImplementedException();
        }
    }
}