using System;
using System.Collections.Generic;
using System.Linq;
using Silent.Practices.DDD;
using Silent.Practices.Extensions;

namespace Silent.Practices.Persistance
{
    public class MemoryRepository<TEntity, TKey> : 
        IRepository<TEntity, TKey> 
        where TEntity : Entity<TKey>
    {
        protected readonly Dictionary<TKey, TEntity> Entities = new Dictionary<TKey, TEntity>();

        public virtual TEntity FindById(TKey id)
        {
            if (!Entities.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            return Entities[id];
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return Entities.Values.ToList();
        }

        public virtual bool Add(TEntity item)
        {
            if (item == null)
            {
                return false;
            }

            if (Entities.ContainsKey(item.EntityId))
            {
                Entities[item.EntityId].Patch(item);
                return true;
            }

            Entities.Add(item.EntityId, item);
            return true;
        }

        public virtual bool Update(TKey key, TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (!Entities.ContainsKey(key))
            {
                return false;
            }

            Entities[key].Patch(entity);
            return true;
        }

        public virtual bool DeleteById(TKey key)
        {
            if (!Entities.ContainsKey(key))
            {
                return false;
            }

            return Entities.Remove(key);
        }

        public bool Delete(TEntity entity)
        {
            return Entities.Remove(entity.EntityId);
        }
    }

    public class MemoryRepository<TEntity> : 
        MemoryRepository<TEntity, Guid>, 
        IRepositoryWithGuidKey<TEntity>
        where TEntity : EntityWithGuidKey
    {
    }
}
