using Silent.Practices.DDD;

namespace Silent.Practices.Persistance
{
    public interface IUnitOfWork
    {
        IRepositoryWithGuidKey<TEntity> GetRepository<TEntity>() where TEntity : EventWithGuidKey;

        bool Commit();

        bool Rollback();
    }
}