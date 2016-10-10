using Silent.Practices.EventStore;
using Silent.Practices.Persistance;

namespace Silent.Practices.Domain.Contracts
{
    public interface IAggregateRepository<TAggregate> : 
        IRepository<TAggregate> 
        where TAggregate : EventAggregate<int>
    {
    }
}