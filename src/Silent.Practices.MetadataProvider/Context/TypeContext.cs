using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class TypeContext
    {
        public TypeContext(TypeInfo source)
        {
            Name = source.Name;
            Properties = new MemberContextSet(source.GetProperties());
        }

        public string Name { get; set; }

        public MemberContextSet Properties { get; }
    }
}