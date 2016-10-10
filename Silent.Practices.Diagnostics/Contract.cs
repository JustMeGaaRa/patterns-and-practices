using System;
using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.Diagnostics
{
    public static class Contract
    {
        public static void NotNull<TSource>(TSource source, string parameterName)
        {
            if (source == null)
            {
                throw new ArgumentNullException($"Parameter {parameterName} cannot be null");
            }
        }

        public static void NotEmpty<TSource>(IEnumerable<TSource> source, string parameterName)
        {
            if (!source?.Any() ?? false)
            {
                throw new ArgumentException($"Parameter {parameterName} cannot be empty collection");
            }
        }
    }
}
