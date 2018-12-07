using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IMemberMetadataBuilder : IContextProvider<MemberContext>, IBuilder<MemberMetadata>
    {
        IMemberMetadataBuilder DisplayAs(string name);

        IMemberMetadataBuilder IsRequired();

        IMemberMetadataBuilder NonEditable();
    }
}