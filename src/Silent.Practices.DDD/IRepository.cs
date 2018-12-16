using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Silent.Practices.Persistance
{
    public interface IRepository<TEntity, in TKey>
    {
        Task<TEntity> FindByIdAsync(TKey id);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<bool> SaveAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteByIdAsync(TKey key);
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
