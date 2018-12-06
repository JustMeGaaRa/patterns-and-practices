using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface ITypeMetadataBuilder : IContextProvider<TypeContext>
    {
        IMemberMetadataBuilder Property(string propertyName);
    }

    public interface ITypeMetadataBuilder<T> : ITypeMetadataBuilder
    {
    }
}