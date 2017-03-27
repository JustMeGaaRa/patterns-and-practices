using Silent.Practices.CQRS.Commands;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeStatefulCommandHandler : ICommandHandler<FakeCommand, FakeStatus>
    {
        public FakeStatus Handle(FakeCommand instance)
        {
            return new FakeStatus();
        }
    }
}
