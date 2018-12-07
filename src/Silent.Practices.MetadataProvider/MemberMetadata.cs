namespace Silent.Practices.MetadataProvider
{
    public class MemberMetadata
    {
        public MemberMetadata(string memberName, TypeMetadata memberType)
        {
            MemberName = memberName;
            MemberType = memberType;
        }

        public MemberMetadata(string memberName, TypeMetadata memberType, string displayName, bool isRequired, bool isEditable)
        {
            MemberName = memberName;
            MemberType = memberType;
            DisplayName = displayName;
            IsRequired = isRequired;
            IsEditable = isEditable;
        }
        
        public string MemberName { get; }

        public TypeMetadata MemberType { get; }

        public string DisplayName { get; set; }

        public bool IsRequired { get; }

        public bool IsEditable { get; }

        public override string ToString() => $"{MemberName} ({MemberType})";
    }
}