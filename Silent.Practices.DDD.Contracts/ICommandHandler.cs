using Silent.Practices.Patterns;

namespace Silent.Practices.Domain.Contracts
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : Command
    {
    }
}
