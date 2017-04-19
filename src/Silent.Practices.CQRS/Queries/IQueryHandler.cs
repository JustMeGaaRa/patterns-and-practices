using Silent.Practices.Patterns;

namespace Silent.Practices.CQRS.Queries
{
    /// <summary>
    /// Defines a contract for handling queries.
    /// </summary>
    /// <typeparam name="TQuery"> Query object type. </typeparam>
    /// <typeparam name="TResult"> Query result data type. </typeparam>
    public interface IQueryHandler<in TQuery, out TResult> : IHandler<TQuery, TResult> 
        where TQuery : IQuery<TResult>
    {
    }
}