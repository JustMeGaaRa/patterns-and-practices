using Silent.Practices.CQRS.Processors;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeStatefulCommandProcessor : StatefulCommandProcessorBase<FakeStatus>
    {
        public FakeStatefulCommandProcessor()
        {
            RegisterHandler<FakeStatefulCommandHandler, FakeCommand>(new FakeStatefulCommandHandler());
        }
    }
}
