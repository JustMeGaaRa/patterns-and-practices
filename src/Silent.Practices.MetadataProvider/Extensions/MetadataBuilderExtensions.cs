using Silent.Practices.MetadataProvider.Builders;

namespace Silent.Practices.MetadataProvider.Extensions
{
    public static class MetadataBuilderExtensions
    {
        public static ITypeMetadataBuilder<T> Entity<T>(this IMetadataBuilder metadataBuilder)
        {
            return new TypeMetadataBuilderWrapper<T>(metadataBuilder.Entity(typeof(T)));
        }
    }
}
