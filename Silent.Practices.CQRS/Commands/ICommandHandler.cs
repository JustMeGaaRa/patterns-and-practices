using Silent.Practices.Patterns;

namespace Silent.Practices.CQRS.Commands
{
    /// <summary>
    /// Defines a contract for handling command with fire-forget style.
    /// </summary>
    /// <typeparam name="TCommand"> Command object type. </typeparam>
    public interface ICommandHandler<in TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {
    }

    /// <summary>
    /// Defines a contract for handling command with execution status result.
    /// </summary>
    /// <typeparam name="TCommand"> Command object type. </typeparam>
    /// <typeparam name="TStatus"> Execution status object type. </typeparam>
    public interface ICommandHandler<in TCommand, out TStatus> : IHandler<TCommand, TStatus>
        where TCommand : ICommand
    {
    }
}