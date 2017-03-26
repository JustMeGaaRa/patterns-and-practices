namespace Silent.Practices.Messaging
{
    public interface IMessageBus<in TMessage> :
        IPublisher<TMessage>,
        ISubscriber<TMessage>
    {
    }
}
