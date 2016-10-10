using System.Collections.Generic;

namespace Silent.Practices.Patterns
{
    public interface IFactory<out TItem>
    {
        IEnumerable<TItem> Create(params object[] parameters);
    }
}
