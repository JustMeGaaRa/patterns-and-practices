namespace Silent.Practices.Persistance
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(params object[] parameters);
    }
}