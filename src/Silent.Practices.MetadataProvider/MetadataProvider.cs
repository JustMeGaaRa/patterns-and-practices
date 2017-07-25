using System;
using System.Linq;
using System.Reflection;
using Silent.Practices.MetadataProvider.Builders;

namespace Silent.Practices.MetadataProvider
{
    public class MetadataProvider : IMetadataProvider
    {
        private readonly IMetadataBuilder _builder;

        public MetadataProvider()
        {
        }

        public MetadataProvider(IMetadataBuilder builder)
        {
            _builder = builder;
        }

        public TypeMetadata GetMetadata<TSource>()
        {
            return GetMetadata(typeof(TSource));
        }

        public TypeMetadata GetMetadata(Type type)
        {
            var builder = _builder ?? new MetadataBuilder();
            return InternalGetTypeMetadata(type, builder);
        }

        private TypeMetadata InternalGetTypeMetadata(Type type, IMetadataBuilder builder)
        {
            var typeContext = builder.Entity(type).GetContext();
            var properties = type.GetTypeInfo().GetProperties();
            var membersMetadataCollection = properties
                .Select(x =>
                {
                    var memberContext = typeContext.Properties.GetProperty(x.Name);
                    var typeMetadata = InternalGetTypeMetadata(x.PropertyType, builder);
                    return new MemberMetadata(memberContext.Name, typeMetadata, memberContext.IsRequired, memberContext.IsEditable);
                })
                .ToList();

            return new TypeMetadata(typeContext.Name, membersMetadataCollection);
        }
    }
}