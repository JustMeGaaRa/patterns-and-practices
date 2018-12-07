using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class TypeContext
    {
        public TypeContext(TypeInfo source)
        {
            Name = source.Name;
            Type = source;
            Properties = new MemberContextSet(source.GetProperties());
        }

        public string Name { get; }

        public TypeInfo Type { get; }

        public MemberContextSet Properties { get; }
    }
}