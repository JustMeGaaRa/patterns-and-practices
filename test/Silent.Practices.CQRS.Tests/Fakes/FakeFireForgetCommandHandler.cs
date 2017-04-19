using Silent.Practices.CQRS.Commands;

namespace Silent.Practices.CQRS.Tests.Fakes
{
    internal sealed class FakeFireForgetCommandHandler : ICommandHandler<FakeCommand>
    {
        public void Handle(FakeCommand instance)
        {
        }
    }
}
