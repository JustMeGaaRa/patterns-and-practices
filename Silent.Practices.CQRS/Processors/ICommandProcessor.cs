using Silent.Practices.CQRS.Commands;

namespace Silent.Practices.CQRS.Processors
{
    /// <summary>
    /// Defines the type for locating and executing correct command handler.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Performs a check if command can be processed by current processor instance.
        /// </summary>
        /// <typeparam name="TCommand"> Command object type. </typeparam>
        /// <param name="command"> Command object instance. </param>
        bool CanProcess<TCommand>(TCommand command) where TCommand : ICommand;

        /// <summary>
        /// Locates and executes handler for specified command instance.
        /// </summary>
        /// <typeparam name="TCommand"> Command result data. </typeparam>
        /// <param name="command"> Command object instance. </param>
        void Process<TCommand>(TCommand command) where TCommand : ICommand;
    }

    /// <summary>
    /// Defines the type for locating and executing correct command handler.
    /// </summary>
    public interface ICommandProcessor<out TStatus>
    {
        /// <summary>
        /// Performs a check if command can be processed by current processor instance.
        /// </summary>
        /// <typeparam name="TCommand"> Command object type. </typeparam>
        /// <param name="command"> Command object instance. </param>
        bool CanProcess<TCommand>(TCommand command) where TCommand : ICommand;

        /// <summary>
        /// Locates and executes handler for specified command instance.
        /// </summary>
        /// <typeparam name="TCommand"> Command result data. </typeparam>
        /// <param name="command"> Command object instance. </param>
        TStatus Process<TCommand>(TCommand command) where TCommand : ICommand;
    }
}