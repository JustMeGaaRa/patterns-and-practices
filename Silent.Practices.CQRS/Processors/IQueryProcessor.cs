using Silent.Practices.CQRS.Queries;

namespace Silent.Practices.CQRS.Processors
{
    /// <summary>
    /// Defines the type for locating and executing correct query handler.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Performs a check if query can be processed by current processor instance.
        /// </summary>
        /// <typeparam name="TResult"> Query result data. </typeparam>
        /// <param name="query"> Query object instance. </param>
        bool CanProcess<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Locates and executes handler for specified query instance.
        /// </summary>
        /// <typeparam name="TResult"> Query result data. </typeparam>
        /// <param name="query"> Query object instance. </param>
        TResult Process<TResult>(IQuery<TResult> query);
    }
}