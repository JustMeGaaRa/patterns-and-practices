using System.Collections.Generic;

namespace Silent.Practices.MetadataProvider
{
    public class TypeCache
    {
        private readonly Dictionary<string, TypeMetadata> _typeContexts = new Dictionary<string, TypeMetadata>();

        public int Count => _typeContexts.Values.Count;

        public TypeMetadata SetType(TypeMetadata type) => _typeContexts[type.TypeName] = type;

        public TypeMetadata GetType(string typeName) => _typeContexts[typeName];
    }
}