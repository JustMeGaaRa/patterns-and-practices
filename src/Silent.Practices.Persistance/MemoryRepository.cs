using System.Collections.Generic;
using System.Linq;
using Silent.Practices.Extensions;

namespace Silent.Practices.Persistance
{
    public class MemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        protected readonly Dictionary<TKey, TEntity> Entities = new Dictionary<TKey, TEntity>();

        public virtual TEntity GetById(TKey id)
        {
            if (!Entities.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            return Entities[id];
        }

        public virtual ICollection<TEntity> Get()
        {
            return Entities.Values.ToList();
        }

        public virtual bool Add(TEntity item)
        {
            if (item == null)
            {
                return false;
            }

            if (Entities.ContainsKey(item.Id))
            {
                Entities[item.Id].Patch(item);
                return true;
            }

            Entities.Add(item.Id, item);
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

        public virtual bool Delete(TKey key)
        {
            if (!Entities.ContainsKey(key))
            {
                return false;
            }

            return Entities.Remove(key);
        }
    }

    public class MemoryRepository<TEntity> : MemoryRepository<TEntity, uint>, IRepository<TEntity>
        where TEntity : IEntity<uint>
    {
    }
}
