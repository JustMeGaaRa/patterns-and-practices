using System.Collections.Generic;

namespace Silent.Practices.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Patch<TKey, TValue>(this IDictionary<TKey, TValue> original, IDictionary<TKey, TValue> modified)
        {
            if (ReferenceEquals(original, modified))
            {
                return;
            }

            original.Clear();

            foreach (var key in modified.Keys)
            {
                original[key] = modified[key];
            }
        }
    }
}