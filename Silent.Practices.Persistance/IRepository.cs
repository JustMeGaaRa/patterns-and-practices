namespace Silent.Practices.Persistance
{
    public interface IRepository<TItem, in TKey> where TItem : EntityBase<TKey>
    {
        int Count { get; }

        TItem GetById(TKey id);

        bool Save(TItem item);
    }

    public interface IRepository<TItem> : IRepository<TItem, int> where TItem : EntityBase<int>
    {
    }
}
