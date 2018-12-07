using Silent.Practices.MetadataProvider.Context;
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

        public TypeContext GetContext() => _context;

        public TypeMetadata Build()
        {
            var type = _context.Type;

            var membersMetadata = type.IsPrimitive || type.IsArray
                ? new List<MemberMetadata>()
                : type.GetTypeInfo()
                    .GetProperties()
                    .Select(x => new MemberMetadata(x.Name, _typeCache.GetType(x.PropertyType.Name)))
                    .ToList();

            return new TypeMetadata(type.Name, membersMetadata);
        }

        public IMemberMetadataBuilder Property(string propertyName)
        {
            var propertyContext = _context.Members.GetProperty(propertyName);
            return new MemberMetadataBuilder(propertyContext, _typeCache);
        }
    }
}