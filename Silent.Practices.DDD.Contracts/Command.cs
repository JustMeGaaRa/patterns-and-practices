namespace Silent.Practices.Domain.Contracts
{
    public abstract class Command
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }
}
