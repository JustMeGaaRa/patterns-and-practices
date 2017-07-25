using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class MemberContext
    {
        public MemberContext(PropertyInfo source)
        {
            Name = source.Name;
            IsRequired = false;
            IsEditable = true;
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public bool IsEditable { get; set; }
    }
}