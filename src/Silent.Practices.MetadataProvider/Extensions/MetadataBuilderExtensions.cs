using Silent.Practices.MetadataProvider.Builders;
using System;

namespace Silent.Practices.MetadataProvider.Extensions
{
    public static class MetadataBuilderExtensions
    {
        public static ITypeMetadataBuilder<TEntity> Entity<TEntity>(this IMetadataBuilder metadataBuilder)
        {
            return new TypeMetadataBuilderWrapper<TEntity>(metadataBuilder.Entity(typeof(TEntity)));
        }

        public static IMetadataBuilder Entity<TEntity>(
            this IMetadataBuilder metadataBuilder, 
            Action<ITypeMetadataBuilder<TEntity>> typeBuilder)
        {
            typeBuilder.Invoke(metadataBuilder.Entity<TEntity>());
            return metadataBuilder;
        }
    }
}
