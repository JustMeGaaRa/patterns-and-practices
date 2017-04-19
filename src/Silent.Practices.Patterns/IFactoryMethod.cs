namespace Silent.Practices.Patterns
{
    public interface IFactoryMethod<out TEntity>
    {
        TEntity Create(params object[] parameters);
    }
}