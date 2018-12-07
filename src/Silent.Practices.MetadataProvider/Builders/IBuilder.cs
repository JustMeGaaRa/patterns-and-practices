namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IBuilder<TResult>
    {
        TResult Build();
    }
}