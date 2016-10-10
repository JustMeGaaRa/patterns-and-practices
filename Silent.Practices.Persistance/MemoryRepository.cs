using System.Collections.Generic;
using Silent.Practices.Diagnostics;
using Silent.Practices.Extensions;

namespace Silent.Practices.Persistance
{
    public sealed class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase<int>
    {
        private readonly Dictionary<int, TEntity> _entities = new Dictionary<int, TEntity>();

        public int Count => _entities.Count;

        public TEntity GetById(int id)
        {
            if (!_entities.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            return _entities[id];
        }

        public bool Save(TEntity item)
        {
            Contract.NotNull(item, nameof(item));

            if (_entities.ContainsKey(item.Id))
            {
                var original = _entities[item.Id];
                original.Patch(item);
                return true;
            }

            _entities.Add(item.Id, item);
            return true;
        }
    }
}
