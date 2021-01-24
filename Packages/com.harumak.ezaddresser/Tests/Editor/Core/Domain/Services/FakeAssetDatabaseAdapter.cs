using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class FakeAssetDatabaseAdapter : IAssetDatabaseAdapter
    {
        public int SaveCount { get; private set; }
        
        private readonly Dictionary<string, string> _assetPathToGuid = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _guidToAssetPath = new Dictionary<string, string>();
        private readonly Dictionary<string, Object> _guidToAsset = new Dictionary<string, Object>();
        private readonly Dictionary<Object, string> _AssetToGuid = new Dictionary<Object, string>();

        public string CreateTestAsset<T>(string assetPath, T asset) where T : Object
        {
            Assert.IsFalse(_assetPathToGuid.ContainsKey(assetPath));

            var guid = GUID.Generate().ToString();
            _assetPathToGuid.Add(assetPath, guid);
            _guidToAssetPath.Add(guid, assetPath);
            _guidToAsset.Add(guid, asset);
            _AssetToGuid.Add(asset, guid);
            return guid;
        }

        public string AssetPathToGUID(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }
            
            if (_assetPathToGuid.TryGetValue(assetPath, out var guid))
            {
                return guid;
            }

            return string.Empty;
        }

        public string GUIDToAssetPath(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }
            
            if (_guidToAssetPath.TryGetValue(guid, out var assetPath))
            {
                return assetPath;
            }

            return string.Empty;
        }

        public string GetAssetPath(Object assetObject)
        {
            if (assetObject == null)
            {
                return null;
            }
            
            if (_AssetToGuid.TryGetValue(assetObject, out var guid))
            {
                return guid;
            }

            return string.Empty;
        }

        public string[] GetAllAssetPaths()
        {
            return _assetPathToGuid.Keys.ToArray();
        }

        public void SaveAssets()
        {
            SaveCount++;
        }

        public Object LoadAssetAtPath(string assetPath, Type type)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }
            
            if (!_assetPathToGuid.TryGetValue(assetPath, out var guid))
            {
                return null;
            }

            if (!_guidToAsset.TryGetValue(guid, out var asset))
            {
                return null;
            }

            return asset;
        }

        public T LoadAssetAtPath<T>(string assetPath) where T : Object
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            if (!_assetPathToGuid.TryGetValue(assetPath, out var guid))
            {
                return null;
            }

            if (!_guidToAsset.TryGetValue(guid, out var asset))
            {
                return null;
            }

            return asset as T;
        }
    }
}
