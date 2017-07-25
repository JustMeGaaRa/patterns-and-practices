using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IMemberMetadataBuilder : IContext<MemberContext>
    {
        IMemberMetadataBuilder IsRequired();
        IMemberMetadataBuilder NonEditable();
    }
}