using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface ITypeMetadataBuilder : IBuilder<TypeMetadata>
    {
        IMemberMetadataBuilder Property(string propertyName);
    }

    public interface ITypeMetadataBuilder<T> : ITypeMetadataBuilder
    {
    }
}