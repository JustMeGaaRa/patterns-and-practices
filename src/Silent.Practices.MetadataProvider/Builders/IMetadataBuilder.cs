using System;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IMetadataBuilder : IBuilder<Metadata>
    {
        ITypeMetadataBuilder Entity(Type type);
    }
}