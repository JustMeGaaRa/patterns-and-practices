using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Silent.Practices.DDD;
using Silent.Practices.Extensions;

namespace Silent.Practices.Persistance
{
    public class MemoryRepository<TEntity, TKey> : 
        IRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
    {
        protected readonly Dictionary<TKey, TEntity> Entities = new Dictionary<TKey, TEntity>();

        public virtual Task<TEntity> FindByIdAsync(TKey id)
        {
            if (!Entities.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            return Task.FromResult(Entities[id]);
        }

        public virtual Task<ICollection<TEntity>> GetAllAsync()
        {
            ICollection<TEntity> entities = Entities.Values.ToList();
            return Task.FromResult(entities);
        }

        public virtual Task<bool> SaveAsync(TEntity item)
        {
            if (item == null)
            {
                return Task.FromResult(false);
            }

            if (Entities.ContainsKey(item.EntityId))
            {
                Entities[item.EntityId].Patch(item);
                return Task.FromResult(true);
            }

            Entities.Add(item.EntityId, item);
            return Task.FromResult(true);
        }

        public virtual Task<bool> Update(TKey key, TEntity entity)
        {
            if (entity == null)
            {
                return Task.FromResult(false);
            }

            if (!Entities.ContainsKey(key))
            {
                return Task.FromResult(false);
            }

            Entities[key].Patch(entity);
            return Task.FromResult(true);
        }

        public virtual Task<bool> DeleteByIdAsync(TKey key)
        {
            if (!Entities.ContainsKey(key))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(Entities.Remove(key));
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Entities.Remove(entity.EntityId));
        }
    }

    public class MemoryRepository<TEntity> : 
        MemoryRepository<TEntity, Guid>, 
        IRepositoryWithGuidKey<TEntity>
        where TEntity : EntityWithGuidKey
    {
    }
}
