namespace Silent.Practices.MetadataProvider
{
    public class MemberMetadata
    {
        public MemberMetadata(string memberName, TypeMetadata memberType, bool isRequired, bool isEditable)
        {
            MemberName = memberName;
            MemberType = memberType;
            IsRequired = isRequired;
            IsEditable = isEditable;
        }
        
        public string MemberName { get; }

        public TypeMetadata MemberType { get; }

        public bool IsRequired { get; }

        public bool IsEditable { get; }

        public override string ToString()
        {
            return $"{MemberType} {MemberName}";
        }
    }
}