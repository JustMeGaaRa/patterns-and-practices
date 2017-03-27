using Silent.Practices.CQRS.Processors;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeQueryProcessor : QueryProcessorBase
    {
        public FakeQueryProcessor()
        {
            RegisterHandler<FakeQueryHandler, FakeQuery, FakeResult>(new FakeQueryHandler());
        }
    }
}
