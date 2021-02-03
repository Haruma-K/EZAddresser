using System;
using System.Collections.Generic;
using System.Linq;
using EZAddresser.Editor.Core.Domain.Adapters;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace EZAddresser.Editor.Core.Infrastructure.Adapters
{
    /// <summary>
    ///     Class for editing Addressables assets.
    /// </summary>
    public class AddressablesEditorAdapter : IAddressablesEditorAdapter
    {
        private readonly AddressableAssetSettings _settings;

        public AddressablesEditorAdapter(AddressableAssetSettings settings)
        {
            _settings = settings;
        }

        public AddressableAssetGroupInfo GetGroupInfo(string groupGuid)
        {
            var group = _settings.FindGroup(x => x.Guid.Equals(groupGuid));
            if (group == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo GetGroupInfoByName(string groupName)
        {
            var group = _settings.FindGroup(groupName);
            if (group == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (Name:{groupName}) because it does not exists.");
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo[] GetGroupInfos()
        {
            return _settings.groups.Select(x => new AddressableAssetGroupInfo(x.Guid, x.Name)).ToArray();
        }

        public bool ExistsGroup(string groupGuid)
        {
            return _settings.FindGroup(x => x.Guid.Equals(groupGuid)) != null;
        }

        public bool ExistsGroupByName(string groupName)
        {
            return _settings.FindGroup(groupName) != null;
        }

        public AddressableAssetGroupInfo CreateGroup(string groupName, bool setAsDefaultGroup, bool readOnly,
            bool postEvent, List<AddressableAssetGroupSchema> schemasToCopy, params Type[] types)
        {
            if (_settings.FindGroup(groupName) != null)
            {
                throw new Exception(
                    $"Failed to create {nameof(AddressableAssetGroup)} (Name:{groupName}) because it is already exists.");
            }

            var group = _settings.CreateGroup(groupName, setAsDefaultGroup, readOnly, postEvent, schemasToCopy, types);
            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public AddressableAssetGroupInfo UpdateGroup(string groupGuid, string name = null,
            BundledAssetGroupSchema.BundlePackingMode? bundlePackingMode = null)
        {
            var group = _settings.FindGroup(x => x.Guid.Equals(groupGuid));
            if (group == null)
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
                foreach (var groupSchema in group.Schemas)
                {
                    var bundledAssetGroupSchema = groupSchema as BundledAssetGroupSchema;
                    if (bundledAssetGroupSchema != null)
                    {
                        bundledAssetGroupSchema.BundleMode = bundlePackingMode.Value;
                    }
                }
            }

            return new AddressableAssetGroupInfo(group.Guid, group.Name);
        }

        public void RemoveGroup(string groupGuid)
        {
            var group = _settings.FindGroup(x => x.Guid.Equals(groupGuid));
            if (group == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            _settings.RemoveGroup(group);
        }

        public AddressableAssetEntryInfo[] GetEntryInfos(string groupGuid)
        {
            var group = _settings.groups.FirstOrDefault(x => x.Guid.Equals(groupGuid));
            if (group != null)
            {
                return group.entries.Select(x => new AddressableAssetEntryInfo(x.guid, x.address)).ToArray();
            }

            return new AddressableAssetEntryInfo[0];
        }

        public AddressableAssetEntryInfo CreateOrMoveEntry(string assetGuid, string groupGuid, bool readOnly = false,
            bool postEvent = true)
        {
            var group = _settings.FindGroup(x => x.Guid.Equals(groupGuid));
            if (group == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetGroup)} (GUID:{groupGuid}) because it does not exists.");
            }

            var entry = _settings.CreateOrMoveEntry(assetGuid, group, readOnly, postEvent);
            return new AddressableAssetEntryInfo(entry.guid, entry.address);
        }

        public void UpdateEntry(string assetGuid, string address = null, string[] labels = null)
        {
            var entry = _settings.FindAssetEntry(assetGuid);
            if (entry == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetEntry)} (GUID:{assetGuid}) because it does not exists.");
            }

            if (address != null)
            {
                entry.SetAddress(address);
            }
            
            if (labels != null)
            {
                entry.labels.Clear();
                foreach (var label in labels)
                {
                    entry.SetLabel(label, true, true);
                }
            }
        }

        public void RemoveAssetEntry(string assetGuid, bool postEvent = true)
        {
            if (_settings.FindAssetEntry(assetGuid) == null)
            {
                throw new Exception(
                    $"Failed to get {nameof(AddressableAssetEntry)} (GUID:{assetGuid}) because it does not exists.");
            }

            _settings.RemoveAssetEntry(assetGuid, postEvent);
        }
    }
}