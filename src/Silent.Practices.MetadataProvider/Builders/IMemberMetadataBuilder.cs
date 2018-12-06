using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface IMemberMetadataBuilder : IContextProvider<MemberContext>
    {
        IMemberMetadataBuilder IsRequired();

        IMemberMetadataBuilder NonEditable();
    }
}