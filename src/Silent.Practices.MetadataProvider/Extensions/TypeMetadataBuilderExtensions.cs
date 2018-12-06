using Silent.Practices.MetadataProvider.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Silent.Practices.MetadataProvider.Extensions
{
    public static class TypeMetadataBuilderExtensions
    {
        public static IMemberMetadataBuilder Property<TEntity>(
            this ITypeMetadataBuilder<TEntity> typeMetadataBuilder, 
            Expression<Func<TEntity, object>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);
            return typeMetadataBuilder.Property(propertyInfo.Name);
        }

        public static ITypeMetadataBuilder<TEntity> HasRequired<TEntity>(
            this ITypeMetadataBuilder<TEntity> typeMetadataBuilder, 
            Expression<Func<TEntity, object>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);
            typeMetadataBuilder.Property(propertyInfo.Name).IsRequired();
            return typeMetadataBuilder;
        }

        public static ITypeMetadataBuilder<TEntity> HasNonEditable<TEntity>(
            this ITypeMetadataBuilder<TEntity> typeMetadataBuilder, 
            Expression<Func<TEntity, object>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);
            typeMetadataBuilder.Property(propertyInfo.Name).NonEditable();
            return typeMetadataBuilder;
        }

        private static PropertyInfo GetPropertyInfo<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyLambda)
        {
            Type type = typeof(TEntity);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            PropertyInfo propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            return propertyInfo;
        }
    }
}
