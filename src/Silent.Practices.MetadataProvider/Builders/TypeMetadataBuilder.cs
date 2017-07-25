using System;
using System.Linq.Expressions;
using System.Reflection;
using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class TypeMetadataBuilder<T> : ITypeMetadataBuilder<T>
    {
        private readonly TypeContext _context;

        public TypeMetadataBuilder(TypeContext context)
        {
            _context = context;
        }

        public IMemberMetadataBuilder Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);
            var propertyContext = _context.Properties.GetProperty(propertyInfo.Name);
            return new MemberMetadataBuilder(propertyContext);
        }

        public ITypeMetadataBuilder<T> HasRequired<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Property(propertyExpression).IsRequired();
            return this;
        }

        public ITypeMetadataBuilder<T> HasNonEditable<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Property(propertyExpression).NonEditable();
            return this;
        }

        public TypeContext GetContext()
        {
            return _context;
        }

        private PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            Type type = typeof(T);

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

    public class TypeMetadataBuilder : ITypeMetadataBuilder
    {
        private readonly TypeContext _context;
        
        public TypeMetadataBuilder(TypeContext context)
        {
            _context = context;
        }

        public IMemberMetadataBuilder Property(string propertyName)
        {
            var propertyContext = _context.Properties.GetProperty(propertyName);
            return new MemberMetadataBuilder(propertyContext);
        }

        public TypeContext GetContext()
        {
            return _context;
        }
    }
}