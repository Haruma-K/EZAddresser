using System.Collections.Generic;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public interface IEntitySet<TEntity> : ICollection<TEntity>
    {
        /// <summary>
        ///     Get the element.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(EntityId id);

        /// <summary>
        ///     Get all elements.
        /// </summary>
        /// <returns></returns>
        TEntity[] GetAll();

        /// <summary>
        ///     Try to get the element.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryGetValue(EntityId id, out TEntity entity);

        /// <summary>
        ///     Delete the element.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(EntityId id);

        /// <summary>
        ///     Return true if the element exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Contains(EntityId id);
    }
}