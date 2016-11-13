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

        public void UseRepository<TEntity>(IRepository<TEntity> repository) where TEntity : EntityBase<uint>
        {
            _repositories[typeof(TEntity)] = repository;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase<uint>
        {
            Type entityType = typeof (TEntity);
            IRepository<TEntity> repository = _repositories.ContainsKey(entityType)
                ? _repositories[entityType] as IRepository<TEntity>
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