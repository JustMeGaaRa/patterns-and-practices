using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MemberMetadataBuilder : IMemberMetadataBuilder
    {
        private readonly MemberContext _context;
        private readonly TypeCache _typeCache;

        public MemberMetadataBuilder(MemberContext context, TypeCache typeCache)
        {
            _context = context;
            _typeCache = typeCache;
        }

        public MemberMetadata Build()
        {
            TypeMetadata typeMetadata = _typeCache.GetType(_context.Name);
            return new MemberMetadata(_context.Name, typeMetadata, _context.DisplayName, _context.IsRequired, _context.IsEditable);
        }

        public IMemberMetadataBuilder DisplayAs(string name)
        {
            _context.DisplayName = name;
            return this;
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
    }
}