namespace Silent.Practices.Persistance
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase<uint>;

        bool Commit();

        bool Rollback();
    }
}