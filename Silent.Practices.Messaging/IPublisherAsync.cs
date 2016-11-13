namespace Silent.Practices.Messaging
{
    public interface IPublisherAsync<in TMessage, out TFuture>
    {
        TFuture PublishAsync<TConcrete>(TConcrete message) where TConcrete : TMessage;
    }
}