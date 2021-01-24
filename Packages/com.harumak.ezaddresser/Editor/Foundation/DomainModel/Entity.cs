using System;
using UnityEngine;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    /// <summary>
    ///     Base class of the Entity.
    /// </summary>
    [Serializable]
    public abstract class Entity
    {
        [SerializeField] private EntityId _id;

        protected Entity()
        {
            _id = EntityId.Create();
        }

        /// <summary>
        ///     ID
        /// </summary>
        public EntityId Id
        {
            get => _id;
            protected internal set => _id = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = (Entity) obj;
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, null)
                ? Equals(right, null)
                : left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{GetType().Name} : {JsonUtility.ToJson(this)}";
        }
    }
}