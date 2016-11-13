namespace Silent.Practices.CQRS
{
    public abstract class Command : ICommand
    {
        protected Command()
        {
        }

        protected Command(uint entityId)
        {
            EntityId = entityId;
        }

        public uint EntityId { get; }
    }
}