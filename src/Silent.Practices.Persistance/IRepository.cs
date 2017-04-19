namespace Silent.Practices.Persistance
{
    public interface IRepository<TEntity, in TKey> where TEntity : EntityBase<TKey>
    {
        TEntity GetById(TKey id);

        bool Save(TEntity item);
    }

    public interface IRepository<TItem> : IRepository<TItem, uint> where TItem : EntityBase<uint>
    {
    }
}
