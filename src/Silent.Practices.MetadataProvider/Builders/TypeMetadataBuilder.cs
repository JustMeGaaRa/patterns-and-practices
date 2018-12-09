using Silent.Practices.MetadataProvider.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class TypeMetadataBuilder : ITypeMetadataBuilder
    {
        private readonly TypeContext _context;
        private readonly TypeCache _typeCache;

        public TypeMetadataBuilder(TypeContext context, TypeCache typeCache)
        {
            _context = context;
            _typeCache = typeCache;
        }

        public TypeMetadata Build() => BuildTypeMetadata(_context.Type, _typeCache);

        public IMemberMetadataBuilder Property(string propertyName)
        {
            var propertyContext = _context.Members.GetProperty(propertyName);
            return new MemberMetadataBuilder(propertyContext, _typeCache);
        }

        private TypeMetadata BuildTypeMetadata(TypeInfo type, TypeCache cache)
        {
            if (cache.ContainsType(type.Name))
            {
                return cache.GetType(type.Name);
            }

            TypeMetadata typeMetadata = null;

            if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime))
            {
                typeMetadata = new TypeMetadata(type.Name);
            }
            else if (type.IsArray)
            {
                typeMetadata = new TypeMetadata(type.Name);
            }
            else if (type.IsGenericType)
            {
                type.GenericTypeArguments.Select(parameter => BuildTypeMetadata(parameter.GetTypeInfo(), cache)).ToList();
                typeMetadata = new TypeMetadata(type.Name, BuildMemberCollection(type, cache));
            }
            else
            {
                typeMetadata = new TypeMetadata(type.Name, BuildMemberCollection(type, cache));
            }

            return cache.SetType(typeMetadata);
        }

        private ICollection<MemberMetadata> BuildMemberCollection(TypeInfo type, TypeCache cache)
        {
            return type.GetProperties().Select(property => BuildMemberMetadata(property, cache)).ToList();
        }

        private MemberMetadata BuildMemberMetadata(PropertyInfo property, TypeCache cache)
        {
            TypeMetadata typeMetadata = BuildTypeMetadata(property.PropertyType.GetTypeInfo(), cache);
            return new MemberMetadata(property.Name, typeMetadata);
        }
    }
}