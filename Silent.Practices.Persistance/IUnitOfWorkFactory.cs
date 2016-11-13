using Silent.Practices.Patterns;

namespace Silent.Practices.Persistance
{
    public interface IUnitOfWorkFactory : IFactoryMethod<IUnitOfWork>
    {
    }
}