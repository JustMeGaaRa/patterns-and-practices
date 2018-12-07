using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class TypeContextSet : IEnumerable<TypeContext>
    {
        private readonly Dictionary<string, TypeContext> _typesContexts = new Dictionary<string, TypeContext>();

        public TypeContextSet() { }

        public TypeContextSet(IEnumerable<TypeInfo> types) => types.Select(AddType).ToList();

        public int Count => _typesContexts.Values.Count;

        public bool ContainsType(TypeInfo type) => _typesContexts.ContainsKey(type.Name);

        public TypeContext AddType(TypeInfo type) => _typesContexts[type.Name] = new TypeContext(type);
        
        public TypeContext GetType(string typeName) => _typesContexts[typeName];

        public IEnumerator<TypeContext> GetEnumerator() => _typesContexts.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}