using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Silent.Practices.DDD;
using Silent.Practices.Extensions;

namespace Silent.Practices.Persistance
{
    public class MemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected readonly Dictionary<TKey, TEntity> Entities = new Dictionary<TKey, TEntity>();

        public virtual Task<TEntity> FindByIdAsync(TKey id)
        {
            if (Entities.ContainsKey(id))
            {
                return Task.FromResult(Entities[id]);
            }

            return Task.FromResult((TEntity)null);
        }

        public virtual Task<ICollection<TEntity>> GetAllAsync()
        {
            ICollection<TEntity> entities = Entities.Values.ToList();
            return Task.FromResult(entities);
        }

        public virtual Task<bool> SaveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (Entities.ContainsKey(entity.EntityId))
            {
                Entities[entity.EntityId].Patch(entity);
                return Task.FromResult(true);
            }

            Entities.Add(entity.EntityId, entity);
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

        public virtual Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

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
