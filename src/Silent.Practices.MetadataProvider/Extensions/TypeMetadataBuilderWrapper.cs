using Silent.Practices.MetadataProvider.Context;
using System;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class TypeMetadataBuilderWrapper<T> : ITypeMetadataBuilder<T>
    {
        private readonly ITypeMetadataBuilder _metadataBuilder;

        public TypeMetadataBuilderWrapper(ITypeMetadataBuilder metadataBuilder) => _metadataBuilder = metadataBuilder;

        public TypeContext GetContext() => _metadataBuilder.GetContext();

        public IMemberMetadataBuilder Property(string propertyName) => _metadataBuilder.Property(propertyName);
    }
}