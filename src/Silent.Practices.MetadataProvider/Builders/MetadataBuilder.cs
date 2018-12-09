using Silent.Practices.MetadataProvider.Context;
using System;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MetadataBuilder : IMetadataBuilder
    {
        private readonly TypeContextSet _context = new TypeContextSet();
        private readonly TypeCache _typeCache;

        public MetadataBuilder(TypeCache typeCache) => _typeCache = typeCache;

        public Metadata Build()
        {
            _context.Select(BuildTypeMetadata).ToList();
            return new Metadata(_typeCache.GetTypes());
        }

        public ITypeMetadataBuilder Entity(Type type)
        {
            var context = _context.ContainsType(type.GetTypeInfo())
                ? _context.GetType(type.Name)
                : _context.AddType(type.GetTypeInfo());
            return new TypeMetadataBuilder(context, _typeCache);
        }

        private TypeMetadata BuildTypeMetadata(TypeContext context)
        {
            return new TypeMetadataBuilder(context, _typeCache).Build();
        }
    }
}