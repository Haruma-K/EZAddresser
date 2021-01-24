using System;
using EZAddresser.Editor.Core.Domain.Adapters;
using UnityEditor;
using Object = UnityEngine.Object;

namespace EZAddresser.Editor.Core.Infrastructure.Adapters
{
    public class AssetDatabaseAdapter : IAssetDatabaseAdapter
    {
        public string AssetPathToGUID(string assetPath)
        {
            return AssetDatabase.AssetPathToGUID(assetPath);
        }

        public string GUIDToAssetPath(string guid)
        {
            return AssetDatabase.GUIDToAssetPath(guid);
        }

        public string GetAssetPath(Object assetObject)
        {
            return AssetDatabase.GetAssetPath(assetObject);
        }

        public string[] GetAllAssetPaths()
        {
            return AssetDatabase.GetAllAssetPaths();
        }

        public void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        public Object LoadAssetAtPath(string assetPath, Type type)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, type);
        }

        public T LoadAssetAtPath<T>(string assetPath) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
}