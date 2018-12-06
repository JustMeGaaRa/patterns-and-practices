using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class TypeMetadataBuilder : ITypeMetadataBuilder
    {
        private readonly TypeContext _context;

        public TypeMetadataBuilder(TypeContext context) => _context = context;

        public IMemberMetadataBuilder Property(string propertyName)
        {
            var propertyContext = _context.Properties.GetProperty(propertyName);
            return new MemberMetadataBuilder(propertyContext);
        }

        public TypeContext GetContext() => _context;
    }
}