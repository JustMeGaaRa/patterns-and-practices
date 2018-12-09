using System.Collections.Generic;

namespace Silent.Practices.MetadataProvider
{
    public class TypeMetadata
    {
        public TypeMetadata(string typeName)
        {
            TypeName = typeName;
            Properties = new List<MemberMetadata>();
        }

        public TypeMetadata(string typeName, ICollection<MemberMetadata> properties)
        {
            TypeName = typeName;
            Properties = properties;
        }

        public string TypeName { get; }

        public ICollection<MemberMetadata> Properties { get; }

        public override string ToString() => $"{TypeName}, Properties: {Properties?.Count}";
    }
}