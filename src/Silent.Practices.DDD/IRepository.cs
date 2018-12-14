using System;
using System.Collections.Generic;

namespace Silent.Practices.Persistance
{
    public interface IRepository<TEntity, in TKey>
    {
        TEntity FindById(TKey id);

        ICollection<TEntity> GetAll();

        bool Add(TEntity entity);

        bool Update(TKey key, TEntity entity);

        bool Delete(TEntity entity);

        bool DeleteById(TKey key);
    }

    public interface IRepositoryWithIntKey<TEntity> : IRepository<TEntity, uint>
    {
    }

    public interface IRepositoryWithStringKey<TEntity> : IRepository<TEntity, string>
    {
    }

    public interface IRepositoryWithGuidKey<TEntity> : IRepository<TEntity, Guid>
    {
    }
}
