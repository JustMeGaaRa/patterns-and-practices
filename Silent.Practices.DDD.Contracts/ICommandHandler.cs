using Silent.Practices.Patterns;

namespace Silent.Practices.Domain.Contracts
{
    public interface ICommandHandler : IHandler<Command>
    {
    }
}
