using System;
using System.Collections.Generic;

namespace Silent.Practices.DDD
{
    public abstract class Entity<TKey>
    {
        public TKey EntityId { get; set; }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(EntityId);
        }

        public override bool Equals(object obj)
        {
            var entityBase = obj as Entity<TKey>;
            return entityBase != null && Equals(entityBase);
        }

        protected bool Equals(Entity<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(EntityId, other.EntityId);
        }
    }

    public abstract class EntityWithGuidKey : Entity<Guid>
    {
        protected EntityWithGuidKey() => EntityId = Guid.NewGuid();
    }

    public abstract class EntityWithStringKey : Entity<string>
    {
        protected EntityWithStringKey() => EntityId = Guid.NewGuid().ToString();
    }

    public abstract class EntityWithIntKey : Entity<int>
    {
    }
}
