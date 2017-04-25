namespace Silent.Practices.Persistance
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}