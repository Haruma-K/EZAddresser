using System;
using Object = UnityEngine.Object;

namespace EZAddresser.Editor.Core.Domain.Adapters
{
    public interface IAssetDatabaseAdapter
    {
        /// <summary>
        ///     <para>Get the GUID for the asset at path.</para>
        /// </summary>
        /// <param name="assetPath">Filesystem path for the asset.</param>
        /// <returns>
        ///     <para>GUID.</para>
        /// </returns>
        string AssetPathToGUID(string assetPath);

        /// <summary>
        ///     <para>Gets the corresponding asset path for the supplied guid, or an empty string if the GUID can't be found.</para>
        /// </summary>
        /// <param name="guid"></param>
        string GUIDToAssetPath(string guid);

        /// <summary>
        ///     <para>Returns the path name relative to the project folder where the asset is stored.</para>
        /// </summary>
        /// <param name="assetObject">A reference to the asset.</param>
        /// <returns>
        ///     <para>The asset path name, or null, or an empty string if the asset does not exist.</para>
        /// </returns>
        string GetAssetPath(Object assetObject);

        /// <summary>
        ///     <para>Returns an array of all asset paths.</para>
        /// </summary>
        string[] GetAllAssetPaths();

        /// <summary>
        ///     <para>Writes all unsaved asset changes to disk.</para>
        /// </summary>
        void SaveAssets();

        /// <summary>
        ///     <para>Returns the first asset object of type type at given path assetPath.</para>
        /// </summary>
        /// <param name="assetPath">Path of the asset to load.</param>
        /// <param name="type">Data type of the asset.</param>
        /// <returns>
        ///     <para>The asset matching the parameters.</para>
        /// </returns>
        Object LoadAssetAtPath(string assetPath, Type type);

        /// <summary>
        ///     <para>Returns the first asset object of type type at given path assetPath.</para>
        /// </summary>
        /// <param name="assetPath">Path of the asset to load.</param>
        /// <typeparam name="T">Data type of the asset.</typeparam>
        /// <returns>
        ///     <para>The asset matching the parameters.</para>
        /// </returns>
        T LoadAssetAtPath<T>(string assetPath) where T : Object;
    }
}