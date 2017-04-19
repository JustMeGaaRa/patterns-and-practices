using Silent.Practices.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace Silent.Practices.CQRS.Processors
{
    public abstract class FireForgetCommandProcessorBase : ICommandProcessor
    {
        protected readonly Dictionary<Type, Action<ICommand>> _handlers = new Dictionary<Type, Action<ICommand>>();

        public bool CanProcess<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _handlers.ContainsKey(command.GetType());
        }

        public void Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (!CanProcess(command))
            {
                throw new ArgumentException($"Cannot process query of type {command.GetType()} because no handler was registered.");
            }

            _handlers[command.GetType()].Invoke(command);
        }

        protected void RegisterHandler<THandler, TCommand>(THandler handler)
            where THandler : class, ICommandHandler<TCommand>
            where TCommand : class, ICommand
        {
            Action<ICommand> genericHandler = (command) => handler.Handle(command as TCommand);
            _handlers[typeof(TCommand)] = genericHandler;
        }
    }
}