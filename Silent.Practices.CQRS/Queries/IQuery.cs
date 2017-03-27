namespace Silent.Practices.CQRS.Queries
{
    /// <summary>
    /// Defines a query object that contains parameters used to query the system for data.
    /// </summary>
    /// <typeparam name="TResult"> Expected query result of type <see cref="TResult"/>. </typeparam>
    public interface IQuery<out TResult>
    {
    }
}
