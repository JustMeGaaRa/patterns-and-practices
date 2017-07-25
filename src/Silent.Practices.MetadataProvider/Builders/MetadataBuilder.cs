using System;
using System.Collections.Generic;
using System.Reflection;
using Silent.Practices.MetadataProvider.Context;

namespace Silent.Practices.MetadataProvider.Builders
{
    public class MetadataBuilder : IMetadataBuilder
    {
        // TODO PH: add a service locator for configured subtypes
        private readonly Dictionary<string, TypeContext> _typeContexts = new Dictionary<string, TypeContext>();

        public ITypeMetadataBuilder Entity(Type type)
        {
            var context = GetTypeContext(type);
            return new TypeMetadataBuilder(context);
        }

        public ITypeMetadataBuilder<T> Entity<T>()
        {
            var context = GetTypeContext(typeof(T));
            return new TypeMetadataBuilder<T>(context);
        }

        private TypeContext GetTypeContext(Type type)
        {
            if (!_typeContexts.ContainsKey(type.FullName))
            {
                _typeContexts.Add(type.FullName, new TypeContext(type.GetTypeInfo()));
            }

            return _typeContexts[type.FullName];
        }
    }
}