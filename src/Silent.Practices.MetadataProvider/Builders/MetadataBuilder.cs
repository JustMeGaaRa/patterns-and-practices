using Silent.Practices.MetadataProvider.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MetadataBuilder : IMetadataBuilder
    {
        private readonly TypeContextSet _types = new TypeContextSet();
        private readonly TypeCache _typeCache;

        public MetadataBuilder() { }

        public MetadataBuilder(TypeCache typeCache) => _typeCache = typeCache;

        public ITypeMetadataBuilder Entity(Type type)
        {
            var context = _types.ContainsType(type.GetTypeInfo())
                ? _types.GetType(type.Name) 
                : _types.AddType(type.GetTypeInfo());
            return new TypeMetadataBuilder(context, _typeCache);
        }
    }
}