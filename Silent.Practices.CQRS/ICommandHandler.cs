using Silent.Practices.Patterns;

namespace Silent.Practices.CQRS
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : ICommand
    {
    }
}