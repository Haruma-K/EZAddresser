using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    /// <summary>
    ///     Manage the collection of the Entity in memory.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [Serializable]
    public class EntitySet<TEntity> : IEntitySet<TEntity>, IReadOnlyEntitySet<TEntity>, ISerializationCallbackReceiver
        where TEntity : Entity
    {
        [SerializeField] private List<TEntity> _entitiesInternal = new List<TEntity>();

        protected Dictionary<EntityId, TEntity> Entities = new Dictionary<EntityId, TEntity>();

        public EntitySet()
        {
        }

        public EntitySet(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public bool IsReadOnly => false;

        public int Count => Entities.Count;

        public TEntity Get(EntityId id)
        {
            var entity = Entities[id];
            return entity;
        }

        public bool TryGetValue(EntityId id, out TEntity entity)
        {
            return Entities.TryGetValue(id, out entity);
        }

        public TEntity[] GetAll()
        {
            return Entities.Values.ToArray();
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            var id = entity.Id;
            if (Entities.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            Entities[id] = entity;
            OnAfterAdd(entity);
        }

        public bool Remove(EntityId id)
        {
            var entity = Entities[id];
            OnBeforeRemove(entity);
            return Entities.Remove(id);
        }

        public bool Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            OnBeforeRemove(entity);
            return Entities.Remove(entity.Id);
        }

        public void Clear()
        {
            Entities.Clear();
        }

        public bool Contains(EntityId id)
        {
            return Entities.ContainsKey(id);
        }

        public bool Contains(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            return Entities.ContainsKey(entity.Id);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            Entities.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Entities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
            _entitiesInternal = Entities.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            Entities = _entitiesInternal.ToDictionary(x => x.Id);
        }

        protected virtual void OnAfterAdd(TEntity entity)
        {
        }

        protected virtual void OnBeforeRemove(TEntity entity)
        {
        }
    }
}