using Silent.Practices.MetadataProvider.Context;
using System;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MetadataBuilder : IMetadataBuilder
    {
        private readonly TypeCache _typeCache;
        private readonly TypeContextSet _typeSet = new TypeContextSet();

        public MetadataBuilder(TypeCache typeCache) => _typeCache = typeCache;

        public ITypeMetadataBuilder Entity(Type type)
        {
            var context = _typeSet.ContainsType(type.GetTypeInfo())
                ? _typeSet.GetType(type.Name)
                : _typeSet.AddType(type.GetTypeInfo());
            return new TypeMetadataBuilder(context, _typeCache);
        }
    }
}