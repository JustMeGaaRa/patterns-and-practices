using Silent.Practices.CQRS.Queries;
using System;
using System.Collections.Generic;

namespace Silent.Practices.CQRS.Processors
{
    public abstract class QueryProcessorBase : IQueryProcessor
    {
        protected readonly Dictionary<Type, Func<object, object>> _handlers = new Dictionary<Type, Func<object, object>>();

        public bool CanProcess<TResult>(IQuery<TResult> query)
        {
            return _handlers.ContainsKey(query.GetType());
        }

        public TResult Process<TResult>(IQuery<TResult> query)
        {
            if(!CanProcess(query))
            {
                throw new ArgumentException($"Cannot process query of type {query.GetType()} because no handler was registered.");
            }

            return (TResult)_handlers[query.GetType()].Invoke(query);
        }

        protected void RegisterHandler<THandler, TQuery, TResult>(THandler handler)
            where THandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : class, IQuery<TResult>
        {
            Func<object, object> genericHandler = (query) => handler.Handle(query as TQuery);
            _handlers[typeof(TQuery)] = genericHandler;
        }
    }
}