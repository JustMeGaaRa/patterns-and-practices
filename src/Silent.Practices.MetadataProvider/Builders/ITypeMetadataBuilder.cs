using System;
using System.Linq.Expressions;
using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public interface ITypeMetadataBuilder<T> : IContext<TypeContext>
    {
        IMemberMetadataBuilder Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
        ITypeMetadataBuilder<T> HasRequired<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
        ITypeMetadataBuilder<T> HasNonEditable<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
    }

    public interface ITypeMetadataBuilder : IContext<TypeContext>
    {
        IMemberMetadataBuilder Property(string propertyName);
    }
}