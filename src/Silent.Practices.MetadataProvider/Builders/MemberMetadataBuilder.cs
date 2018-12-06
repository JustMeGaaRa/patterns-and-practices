using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MemberMetadataBuilder : IMemberMetadataBuilder
    {
        private readonly MemberContext _context;

        public MemberMetadataBuilder(MemberContext context)
        {
            _context = context;
        }

        public IMemberMetadataBuilder IsRequired()
        {
            _context.IsRequired = true;
            return this;
        }

        public IMemberMetadataBuilder NonEditable()
        {
            _context.IsEditable = false;
            return this;
        }

        public MemberContext GetContext() => _context;
    }
}