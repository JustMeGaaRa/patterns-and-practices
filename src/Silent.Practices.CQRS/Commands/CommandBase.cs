namespace Silent.Practices.CQRS.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase()
        {
        }

        protected CommandBase(uint entityId)
        {
            EntityId = entityId;
        }

        public uint EntityId { get; }
    }
}