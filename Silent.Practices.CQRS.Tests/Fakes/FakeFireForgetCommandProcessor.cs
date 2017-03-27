using Silent.Practices.CQRS.Processors;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeFireForgetCommandProcessor : FireForgetCommandProcessorBase
    {
        public FakeFireForgetCommandProcessor()
        {
            RegisterHandler<FakeFireForgetCommandHandler, FakeCommand>(new FakeFireForgetCommandHandler());
        }
    }
}
