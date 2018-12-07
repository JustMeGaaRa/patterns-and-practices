using System.Reflection;

namespace Silent.Practices.MetadataProvider.Context
{
    public class MemberContext
    {
        public MemberContext(PropertyInfo source)
        {
            Name = source.Name;
            Type = source;
            DisplayName = source.Name;
            IsRequired = false;
            IsEditable = source.CanWrite;
        }

        public string Name { get; }

        public PropertyInfo Type { get; }

        public string DisplayName { get; set; }

        public bool IsRequired { get; set; }

        public bool IsEditable { get; set; }
    }
}