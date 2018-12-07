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
            PropertyInfo propertyInfo = null;

            // TODO PH: replace this with recursive pattern matching
            switch (propertyLambda.Body)
            {
                case MemberExpression expression:
                    propertyInfo = expression.Member as PropertyInfo;

                    break;
                case UnaryExpression expression:
                    MemberExpression propertyExpression = expression.Operand as MemberExpression;
                    propertyInfo = propertyExpression?.Member as PropertyInfo;
                    break;
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            return propertyInfo;
        }
    }
}
