namespace Silent.Practices.MetadataProvider.Extensions
{
    public static class MetadataProviderExtensions
    {
        public static TypeMetadata GetMetadata<TSource>(this IMetadataProvider metadataProvider)
        {
            return metadataProvider.GetMetadata(typeof(TSource));
        }
    }
}
