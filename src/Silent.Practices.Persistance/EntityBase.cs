using System.Collections.Generic;

namespace Silent.Practices.Persistance
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            var entityBase = obj as EntityBase<TKey>;
            return entityBase != null && Equals(entityBase);
        }

        protected bool Equals(EntityBase<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }
    }
}
