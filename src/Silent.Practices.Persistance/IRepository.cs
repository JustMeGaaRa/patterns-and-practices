using System.Collections.Generic;

namespace Silent.Practices.Persistance
{
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
    {
        TEntity GetById(TKey id);

        ICollection<TEntity> Get();

        bool Add(TEntity item);

        bool Update(TKey key, TEntity entity);

        bool Delete(TKey key);
    }

    public interface IRepository<TItem> : IRepository<TItem, uint> where TItem : IEntity<uint>
    {
    }
}
