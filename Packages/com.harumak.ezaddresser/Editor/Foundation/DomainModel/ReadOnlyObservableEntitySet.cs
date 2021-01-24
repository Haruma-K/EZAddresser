using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EZAddresser.Editor.Foundation.Observable;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public class ReadOnlyObservableEntitySet<TEntity, TReadOnlyEntity> : IReadOnlyObservableEntitySet<TReadOnlyEntity>
        where TEntity : Entity
        where TReadOnlyEntity : Entity
    {
        private readonly CollectionItemConverter<TEntity, TReadOnlyEntity> _converter;
        protected readonly IObservableEntitySet<TEntity> Source;

        public ReadOnlyObservableEntitySet(IObservableEntitySet<TEntity> source,
            Func<TEntity, TReadOnlyEntity> converter)
        {
            Source = source;
            _converter = new CollectionItemConverter<TEntity, TReadOnlyEntity>(converter);
        }

        public int Count => Source.Count;

        public bool Contains(EntityId id)
        {
            return Source.Contains(id);
        }

        public TReadOnlyEntity Get(EntityId id)
        {
            var entity = Source.Get(id);
            return _converter.Invoke(entity);
        }

        public bool TryGetValue(EntityId id, out TReadOnlyEntity entity)
        {
            if (Source.TryGetValue(id, out var sourceItem))
            {
                entity = _converter.Invoke(sourceItem);
                return true;
            }

            entity = default;
            return false;
        }

        public IObservable<EntitySetAddEvent<TReadOnlyEntity>> ObservableAdd
        {
            get
            {
                var observableAdd = Source.ObservableAdd;
                return observableAdd.Convert(x =>
                    new EntitySetAddEvent<TReadOnlyEntity>(_converter.Invoke(x.Value)));
            }
        }

        public IObservable<EntitySetRemoveEvent<TReadOnlyEntity>> ObservableRemove
        {
            get
            {
                var observableRemove = Source.ObservableRemove;
                return observableRemove.Convert(x =>
                    new EntitySetRemoveEvent<TReadOnlyEntity>(_converter.Invoke(x.Value)));
            }
        }

        public IObservable<Empty> ObservableClear
        {
            get
            {
                var observableClear = Source.ObservableClear;
                return observableClear;
            }
        }

        public IEnumerator<TReadOnlyEntity> GetEnumerator()
        {
            return Source
                .Select(sourceItem => _converter.Invoke(sourceItem))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}