using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class TypeMetadataBuilderWrapper<T> : ITypeMetadataBuilder<T>
    {
        private readonly ITypeMetadataBuilder _metadataBuilder;

        public TypeMetadataBuilderWrapper(ITypeMetadataBuilder metadataBuilder) => _metadataBuilder = metadataBuilder;

        public TypeMetadata Build() => _metadataBuilder.Build();

        public IMemberMetadataBuilder Property(string propertyName) => _metadataBuilder.Property(propertyName);
    }
}