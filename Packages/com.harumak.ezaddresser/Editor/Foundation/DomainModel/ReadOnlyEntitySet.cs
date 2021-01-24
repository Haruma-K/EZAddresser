using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public class ReadOnlyEntitySet<TEntityDto> : IReadOnlyEntitySet<TEntityDto>
        where TEntityDto : Entity
    {
        private readonly CollectionItemConverter<EntityId, TEntityDto> _converter;
        private readonly ICollection<EntityId> _source;

        public ReadOnlyEntitySet(ICollection<EntityId> source, Func<EntityId, TEntityDto> converter)
        {
            _source = source;
            _converter = new CollectionItemConverter<EntityId, TEntityDto>(converter);
        }

        public int Count => _source.Count;

        public bool Contains(EntityId id)
        {
            return _source.Contains(id);
        }

        public TEntityDto Get(EntityId id)
        {
            return _converter.Invoke(id);
        }

        public bool TryGetValue(EntityId id, out TEntityDto entity)
        {
            if (Contains(id))
            {
                entity = Get(id);
                return true;
            }

            entity = default;
            return false;
        }

        public IEnumerator<TEntityDto> GetEnumerator()
        {
            return _source
                .Select(sourceItem => _converter.Invoke(sourceItem))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EntitySetDto<TEntity, TEntityDto> : IReadOnlyEntitySet<TEntityDto>
        where TEntity : Entity
        where TEntityDto : Entity
    {
        private readonly CollectionItemConverter<TEntity, TEntityDto> _converter;
        private readonly IEntitySet<TEntity> _source;

        public EntitySetDto(IEntitySet<TEntity> source, Func<TEntity, TEntityDto> converter)
        {
            _source = source;
            _converter = new CollectionItemConverter<TEntity, TEntityDto>(converter);
        }

        public int Count => _source.Count;

        public bool Contains(EntityId id)
        {
            return _source.Contains(id);
        }

        public TEntityDto Get(EntityId id)
        {
            return _converter.Invoke(_source.Get(id));
        }

        public bool TryGetValue(EntityId id, out TEntityDto entity)
        {
            if (_source.TryGetValue(id, out var sourceItem))
            {
                entity = _converter.Invoke(sourceItem);
                return true;
            }

            entity = default;
            return false;
        }

        public IEnumerator<TEntityDto> GetEnumerator()
        {
            return _source
                .Select(sourceItem => _converter.Invoke(sourceItem))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TEntityDto GetSingle()
        {
            return _converter.Invoke(_source.Single());
        }
    }
}