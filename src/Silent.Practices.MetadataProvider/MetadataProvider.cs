using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Silent.Practices.MetadataProvider.Builders;
using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider
{
    public class MetadataProvider : IMetadataProvider
    {
        private readonly IMetadataBuilder _builder;

        public MetadataProvider() => _builder = new MetadataBuilder();

        public MetadataProvider(IMetadataBuilder builder) => _builder = builder;

        public TypeMetadata GetMetadata(Type type) => InternalGetTypeMetadata(type);

        private TypeMetadata InternalGetTypeMetadata(Type type)
        {
            var typeContext = _builder.Entity(type).GetContext();
            var mapToMemberMetadataPartial = new Func<PropertyInfo, MemberMetadata>(x => MapPropertyInfoToMetadata(typeContext, x));
            var membersMetadataCollection = type.IsPrimitive || type.IsArray
                ? new List<MemberMetadata>()
                : type.GetTypeInfo()
                    .GetProperties()
                    .Select(mapToMemberMetadataPartial)
                    .ToList();

            return new TypeMetadata(typeContext.Name, membersMetadataCollection);
        }

        private MemberMetadata MapPropertyInfoToMetadata(TypeContext typeContext, PropertyInfo propertyInfo)
        {
            var memberContext = typeContext.Properties.GetProperty(propertyInfo.Name);
            var typeMetadata = InternalGetTypeMetadata(propertyInfo.PropertyType);
            return new MemberMetadata(memberContext.Name, typeMetadata, memberContext.IsRequired, memberContext.IsEditable);
        }
    }
}