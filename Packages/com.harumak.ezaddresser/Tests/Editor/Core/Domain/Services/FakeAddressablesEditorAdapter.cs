using System;
using System.Collections.Generic;
using System.Linq;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Services;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    /// <summary>
    ///     Fake class for editing Addressables assets.
    /// </summary>
    public class FakeAddressablesEditorAdapter : IAddressablesEditorAdapter
    {
        private readonly Dictionary<string, FakeAddressableAssetGroup> _groups =
            new Dictionary<string, FakeAddressableAssetGroup>();

        public Dictionary<string, FakeAddressableAssetGroup> Groups => _groups;

        public AddressableAssetGroupInfo GetGroupInfo(string groupGuid)
        {
            if (!_groups.TryGetValue(groupGuid, out var group))
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo GetGroupInfoByName(string groupName)
        {
            var group = _groups.Values.FirstOrDefault(x => x.Name.Equals(groupName));
            if (group == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (Name:{groupName}) because it does not exists.");
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo[] GetGroupInfos()
        {
            return _groups.Values.Select(x => new AddressableAssetGroupInfo(x.Guid, x.Name)).ToArray();
        }

        public bool ExistsGroup(string groupGuid)
        {
            return _groups.Keys.Contains(groupGuid);
        }

        public bool ExistsGroupByName(string groupName)
        {
            return _groups.Values.Any(x => x.Name.Equals(groupName));
        }

        public AddressableAssetGroupInfo CreateGroup(string groupName, bool setAsDefaultGroup, bool readOnly,
            bool postEvent, List<AddressableAssetGroupSchema> schemasToCopy, params Type[] types)
        {
            if (_groups.Values.Any(x => x.Name.Equals(groupName)))
            {
                throw new Exception(
                    $"Failed to create {nameof(AddressableAssetGroup)} (Name:{groupName}) because it is already exists.");
            };
            var group = new FakeAddressableAssetGroup(groupName);
            _groups.Add(group.Guid, group);
            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo UpdateGroup(string groupGuid, string name = null,
            BundledAssetGroupSchema.BundlePackingMode? bundlePackingMode = null)
        {
            if (!_groups.TryGetValue(groupGuid, out var group))
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            if (name != null)
            {
                group.Name = name;
            }

            if (bundlePackingMode.HasValue)
            {
                group.BundlePackingMode = bundlePackingMode.Value;
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public void RemoveGroup(string groupGuid)
        {
            if (!_groups.Keys.Contains(groupGuid))
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            _groups.Remove(groupGuid);
        }

        public AddressableAssetEntryInfo[] GetEntryInfos(string groupGuid)
        {
            if (!_groups.TryGetValue(groupGuid, out var group))
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            return group.Entries.Select(x => new AddressableAssetEntryInfo(x.Guid, x.Address)).ToArray();
        }

        public void UpdateEntry(string assetGuid, string address = null, string[] labels = null)
        {
            foreach (var group in _groups.Values)
            {
                var entry = group.Entries.Find(x => x.Guid.Equals(assetGuid));
                if (entry == null)
                {
                    continue;
                }

                if (address != null)
                {
                    entry.Address = address;
                }

                if (labels != null)
                {
                    entry.Labels = labels;
                }
                return;
            }

            throw new Exception(
                $"Failed to get {nameof(AddressableAssetEntry)} (GUID:{assetGuid}) because it does not exists.");
        }

        public AddressableAssetEntryInfo CreateOrMoveEntry(string assetGuid, string groupGuid, bool readOnly = false,
            bool postEvent = true)
        {
            // Remove old entry.
            var beforeGroup = _groups.Values.FirstOrDefault(x => x.Entries.Any(y => y.Guid.Equals(assetGuid)));
            if (beforeGroup != null)
            {
                var oldEntry = beforeGroup.Entries.FirstOrDefault(x => x.Guid.Equals(assetGuid));
                beforeGroup.Entries.Remove(oldEntry);
            }
            
            if (!_groups.TryGetValue(groupGuid, out var group))
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }
            
            // Add new entry.
            group.Entries.Add(new FakeAddressableAssetEntry(assetGuid));
            return new AddressableAssetEntryInfo(assetGuid, string.Empty);
        }

        public void RemoveAssetEntry(string assetGuid, bool postEvent = true)
        {
            foreach (var group in _groups.Values)
            {
                var entry = group.Entries.FirstOrDefault(x => x.Guid.Equals(assetGuid));
                if (entry != null)
                {
                    group.Entries.Remove(entry);
                    return;
                }
            }

            throw new Exception(
                $"Failed to get {nameof(AddressableAssetEntry)} (GUID:{assetGuid}) because it does not exists.");
        }

        public class FakeAddressableAssetGroup
        {
            public BundledAssetGroupSchema.BundlePackingMode BundlePackingMode =
                BundledAssetGroupSchema.BundlePackingMode.PackTogether;

            public readonly List<FakeAddressableAssetEntry> Entries = new List<FakeAddressableAssetEntry>();

            public FakeAddressableAssetGroup(string name)
            {
                Guid = GUID.Generate().ToString();
                Name = name;
            }

            public string Guid { get; }
            public string Name { get; set; }
        }

        public class FakeAddressableAssetEntry
        {
            public FakeAddressableAssetEntry(string guid)
            {
                Guid = guid;
            }

            public string Guid { get; }
            public string Address { get; set; }
            public string[] Labels { get; set; }
        }
    }
}