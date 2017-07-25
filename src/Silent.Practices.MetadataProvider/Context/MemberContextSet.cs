using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class MemberContextSet : IEnumerable<MemberContext>
    {
        private readonly Dictionary<string, MemberContext> _memberContexts = new Dictionary<string, MemberContext>();

        public MemberContextSet()
        {
        }

        public MemberContextSet(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                AddProperty(property);
            }
        }

        public int Count
        {
            get { return _memberContexts.Values.Count; }
        }

        public MemberContext AddProperty(PropertyInfo property)
        {
            if(!_memberContexts.ContainsKey(property.Name))
            {
                _memberContexts[property.Name] = new MemberContext(property);
            }

            return _memberContexts[property.Name];
        }

        public MemberContext GetProperty(string propertyName)
        {
            // TODO PH: verify that exceptions do not occur here
            return _memberContexts[propertyName];
        }

        public IEnumerator<MemberContext> GetEnumerator()
        {
            return _memberContexts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}