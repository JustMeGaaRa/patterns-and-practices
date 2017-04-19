namespace Silent.Practices.Messaging
{
    public interface IPublisher<in TMessage>
    {
        void Publish<TConcrete>(TConcrete message) where TConcrete : TMessage;
    }
}