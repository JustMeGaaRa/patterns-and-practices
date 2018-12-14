using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Silent.Practices.DDD
{
    public abstract class Enumeration : IComparable
    {
        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public int Id { get; }

        public string Name { get; }

        public override string ToString() => Name;

        public static ICollection<T> GetAll<T>() where T : Enumeration, new()
        {
            // TODO: check the possibility to create an instance internally
            var type = typeof(T);
            var fields = type.GetTypeInfo().GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly);

            List<T> enumerations = new List<T>();

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;
                if (locatedValue != null)
                {
                    enumerations.Add(locatedValue);
                }
            }

            return enumerations;
        }

        public static T Parse<T>(string name) where T : Enumeration, new()
        {
            return GetAll<T>().FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;
            if (otherValue == null)
            {
                return false;
            }
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Enumeration enumeration = obj as Enumeration;
            return Id.CompareTo(enumeration?.Id);
        }
    }
}
