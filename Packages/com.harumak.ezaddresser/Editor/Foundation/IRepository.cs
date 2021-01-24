using System.Threading.Tasks;

namespace EZAddresser.Editor.Foundation
{
    /// <summary>
    ///     Interface of the repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        ///     Return true if the repository exists. If false, you need to call <see cref="Save" /> first.
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        ///     Fetch saved item.
        /// </summary>
        /// <returns></returns>
        T Fetch();

        /// <summary>
        ///     Fetch saved item asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<T> FetchAsync();

        /// <summary>
        ///     Save the item.
        /// </summary>
        /// <param name="item"></param>
        void Save(T item);

        /// <summary>
        ///     Save the item asynchronously.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task SaveAsync(T item);
    }
}