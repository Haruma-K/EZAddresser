using System;
using System.Collections;
using System.Collections.Generic;
using EZAddresser.Editor.Foundation.Observable;
using UnityEngine;
using UnityEngine.Assertions;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    [Serializable]
    public class ObservableEntitySet<TEntity> : IObservableEntitySet<TEntity>, IReadOnlyObservableEntitySet<TEntity>
        where TEntity : Entity
    {
        [SerializeField] protected EntitySet<TEntity> internalSet = new EntitySet<TEntity>();

        private readonly Subject<EntitySetAddEvent<TEntity>> _subjectAdd = new Subject<EntitySetAddEvent<TEntity>>();
        private readonly Subject<Empty> _subjectClear = new Subject<Empty>();

        private readonly Subject<EntitySetRemoveEvent<TEntity>> _subjectRemove =
            new Subject<EntitySetRemoveEvent<TEntity>>();

        private bool _didDispose;

        public ObservableEntitySet()
        {
        }

        public ObservableEntitySet(IEnumerable<TEntity> entities)
        {
            Assert.IsNotNull(entities);

            foreach (var entity in entities)
            {
                internalSet.Add(entity);
            }
        }

        public IObservable<EntitySetAddEvent<TEntity>> ObservableAdd => _subjectAdd;

        public IObservable<EntitySetRemoveEvent<TEntity>> ObservableRemove => _subjectRemove;

        public IObservable<Empty> ObservableClear => _subjectClear;

        public bool IsReadOnly => false;

        public TEntity Get(EntityId id)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(id == null);

            return internalSet.Get(id);
        }

        public bool TryGetValue(EntityId id, out TEntity entity)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(id == null);

            return internalSet.TryGetValue(id, out entity);
        }

        public TEntity[] GetAll()
        {
            Assert.IsFalse(_didDispose);

            return internalSet.GetAll();
        }

        public void Add(TEntity item)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(item == null);

            internalSet.Add(item);
            _subjectAdd.OnNext(new EntitySetAddEvent<TEntity>(item));
            OnAfterAdd(item);
        }

        public bool Remove(TEntity entity)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(entity == null);

            OnBeforeRemove(entity);
            internalSet.Remove(entity);
            _subjectRemove.OnNext(new EntitySetRemoveEvent<TEntity>(entity));
            return true;
        }

        public bool Remove(EntityId id)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsFalse(id == null);

            var entity = internalSet.Get(id);
            OnBeforeRemove(entity);
            internalSet.Remove(id);
            _subjectRemove.OnNext(new EntitySetRemoveEvent<TEntity>(entity));
            return true;
        }

        public void Clear()
        {
            Assert.IsFalse(_didDispose);

            internalSet.Clear();
            _subjectClear.OnNext(Empty.Default);
        }

        public int Count => internalSet.Count;


        public bool Contains(TEntity entity)
        {
            Assert.IsFalse(_didDispose);

            if (entity == null)
            {
                return false;
            }

            return internalSet.Contains(entity);
        }

        public bool Contains(EntityId id)
        {
            Assert.IsFalse(_didDispose);

            if (id == null)
            {
                return false;
            }

            return internalSet.Contains(id);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            Assert.IsFalse(_didDispose);
            Assert.IsNotNull(array);

            internalSet.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return internalSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalSet.GetEnumerator();
        }

        public void Dispose()
        {
            Assert.IsFalse(_didDispose);

            DisposeSubject(_subjectAdd);
            DisposeSubject(_subjectRemove);
            DisposeSubject(_subjectClear);

            _didDispose = true;
        }

        protected virtual void OnAfterAdd(TEntity entity)
        {
        }

        protected virtual void OnBeforeRemove(TEntity entity)
        {
        }

        private static void DisposeSubject<TSubjectValue>(Subject<TSubjectValue> subject)
        {
            if (subject.DidDispose)
            {
                return;
            }

            if (subject.DidTerminate)
            {
                subject.Dispose();
                return;
            }

            subject.OnCompleted();
            subject.Dispose();
        }
    }
}