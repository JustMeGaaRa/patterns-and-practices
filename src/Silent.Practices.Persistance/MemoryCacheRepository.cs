using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Silent.Practices.DDD;

namespace Silent.Practices.Persistance
{
    public class MemoryCacheRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository<TEntity, TKey> _repository;

        public MemoryCacheRepository(IMemoryCache cache, IRepository<TEntity, TKey> repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public Task<TEntity> FindByIdAsync(TKey id)
        {
            return _cache.TryGetValue(id, out TEntity value)
                ? Task.FromResult(value)
                : _repository.FindByIdAsync(id);
        }

        public Task<ICollection<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<bool> SaveAsync(TEntity entity)
        {
            if (entity == null)
            {
                return Task.FromResult(false);
            }

            _cache.Set(entity.EntityId, entity);
            return _repository.SaveAsync(entity);
        }

        public Task<bool> DeleteByIdAsync(TKey key)
        {
            _cache.Remove(key);
            return _repository.DeleteByIdAsync(key);
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            _cache.Remove(entity.EntityId);
            return _repository.DeleteAsync(entity);
        }
    }

    public class MemoryCacheRepository<TEntity> :
        MemoryCacheRepository<TEntity, Guid>,
        IRepositoryWithGuidKey<TEntity>
        where TEntity : EntityWithGuidKey
    {
        public MemoryCacheRepository(IMemoryCache cache, IRepository<TEntity, Guid> repository) 
            : base(cache, repository)
        {
        }
    }
}
