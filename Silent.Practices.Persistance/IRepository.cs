namespace Silent.Practices.Persistance
{
    public interface IRepository<TItem, in TKey> where TItem : EntityBase<TKey>
    {
        TItem GetById(TKey id);

        bool Save(TItem item);
    }

    public interface IRepository<TItem> : IRepository<TItem, uint> where TItem : EntityBase<uint>
    {
    }
}
