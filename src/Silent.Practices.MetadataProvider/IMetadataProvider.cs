using System;

namespace Silent.Practices.MetadataProvider
{
    public interface IMetadataProvider
    {
        TypeMetadata GetMetadata(Type type);
        TypeMetadata GetMetadata<TSource>();
    }
}