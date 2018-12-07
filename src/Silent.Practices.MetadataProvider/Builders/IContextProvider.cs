namespace Silent.Practices.MetadataProvider.Context
{
    public interface IContextProvider<T>
    {
        T GetContext();
    }
}