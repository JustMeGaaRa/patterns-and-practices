namespace Silent.Practices.MetadataProvider.Context
{
    public interface IContext<T>
    {
        T GetContext();
    }
}