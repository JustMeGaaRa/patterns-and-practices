using Silent.Practices.CQRS.Queries;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeQueryHandler : IQueryHandler<FakeQuery, FakeResult>
    {
        public FakeResult Handle(FakeQuery instance)
        {
            return new FakeResult();
        }
    }
}
