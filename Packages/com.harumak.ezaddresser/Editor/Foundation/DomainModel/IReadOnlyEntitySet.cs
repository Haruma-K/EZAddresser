using System.Collections.Generic;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public interface IReadOnlyEntitySet<TEntity> : IReadOnlyCollection<TEntity> where TEntity : Entity
    {
        /// <summary>
        ///     Get the element.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(EntityId id);

        /// <summary>
        ///     Try to get the element.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryGetValue(EntityId id, out TEntity entity);

        /// <summary>
        ///     Return true if the element exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(EntityId id);
    }
}