using Silent.Practices.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace Silent.Practices.CQRS.Processors
{
    public abstract class StatefulCommandProcessorBase<TStatus> : ICommandProcessor<TStatus>
    {
        protected readonly Dictionary<Type, Func<ICommand, TStatus>> _handlers = new Dictionary<Type, Func<ICommand, TStatus>>();

        public bool CanProcess<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _handlers.ContainsKey(command.GetType());
        }

        public TStatus Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (!CanProcess(command))
            {
                throw new ArgumentException($"Cannot process query of type {command.GetType()} because no handler was registered.");
            }

            return _handlers[command.GetType()].Invoke(command);
        }

        protected void RegisterHandler<THandler, TCommand>(THandler handler)
            where THandler : class, ICommandHandler<TCommand, TStatus>
            where TCommand : class, ICommand
        {
            Func<ICommand, TStatus> genericHandler = (command) => handler.Handle(command as TCommand);
            _handlers[typeof(TCommand)] = genericHandler;
        }
    }
}