﻿using System.Threading.Tasks;

namespace EZAddresser.Editor.Foundation.LocalPersistence
{
    internal interface ILocalPersistence<T>
    {
        /// <summary>
        ///     Persist the data to local storage.
        /// </summary>
        /// <param name="target"></param>
        Task SaveAsync(T target);

        /// <summary>
        ///     Load the data from local storage.
        /// </summary>
        /// <returns></returns>
        Task<T> LoadAsync();

        /// <summary>
        ///     Delete the data in local storage.
        /// </summary>
        void Delete();

        /// <summary>
        ///     Return true if the data exists in local storage.
        /// </summary>
        bool Exists();

        /// <summary>
        ///     Get the data path in local storage.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetPath();
    }
}