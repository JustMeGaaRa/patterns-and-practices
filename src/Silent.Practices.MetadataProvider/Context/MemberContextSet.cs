using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class MemberContextSet : IEnumerable<MemberContext>
    {
        private readonly Dictionary<string, MemberContext> _memberContexts = new Dictionary<string, MemberContext>();

        public MemberContextSet() { }

        public MemberContextSet(IEnumerable<PropertyInfo> properties) => properties.Select(AddProperty).ToList();

        public int Count => _memberContexts.Values.Count;

        public MemberContext AddProperty(PropertyInfo property)
        {
            if (!_memberContexts.ContainsKey(property.Name))
            {
                _memberContexts[property.Name] = new MemberContext(property);
            }

            return _memberContexts[property.Name];
        }

        // TODO PH: verify that exceptions do not occur here
        public MemberContext GetProperty(string propertyName) => _memberContexts[propertyName];

        public IEnumerator<MemberContext> GetEnumerator() => _memberContexts.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}