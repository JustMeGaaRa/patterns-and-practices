using System;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IMetadataBuilder
    {
        ITypeMetadataBuilder Entity(Type type);
    }
}