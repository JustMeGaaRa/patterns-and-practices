using System;

namespace Silent.Practices.Patterns
{
    public static class Actions<T>
    {
        public static readonly Action<T> Ignore = x => {};

        public static readonly Func<T, T> Propagate = x => x;
    }
}