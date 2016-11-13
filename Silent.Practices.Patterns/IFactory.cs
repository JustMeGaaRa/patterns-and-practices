using System.Collections.Generic;

namespace Silent.Practices.Patterns
{
    public interface IFactory<out TEntity>
    {
        IEnumerable<TEntity> Create(params object[] parameters);
    }
}
