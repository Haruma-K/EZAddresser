using System;
using System.Linq;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryOperationInfos;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace EZAddresser.Editor.Core.Domain.Services
{
    /// <summary>
    ///     This class is used to apply <see cref="EntryOperationInfo" /> to Addressables.
    /// </summary>
    public class EntryOperationInfoApplyDomainService
    {
        private readonly IAddressablesEditorAdapter _addressablesEditorAdapter;
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="addressablesEditorAdapter"></param>
        /// <param name="assetDatabaseAdapter"></param>
        public EntryOperationInfoApplyDomainService(IAddressablesEditorAdapter addressablesEditorAdapter,
            IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            _addressablesEditorAdapter = addressablesEditorAdapter;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        /// <summary>
        ///     Apply a <see cref="EntryOperationInfo" /> to the <see cref="AddressableAssetSettings" />.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>True if processed.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Apply(EntryOperationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Assert.IsFalse(info.CreateOrMoveInfo == null && info.RemoveInfo == null);

            var processed = false;
            if (info.CreateOrMoveInfo != null)
            {
                processed |= ApplyCreateOrMoveInfo(info.CreateOrMoveInfo);
            }

            if (info.RemoveInfo != null)
            {
                processed |= ApplyRemoveInfo(info.RemoveInfo);
            }

            return processed;
        }

        private bool ApplyCreateOrMoveInfo(EntryCreateOrMoveOperationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Assert.IsFalse(string.IsNullOrEmpty(info.AssetPath));
            Assert.IsFalse(string.IsNullOrEmpty(info.Address));
            Assert.IsFalse(string.IsNullOrEmpty(info.GroupName));
            Assert.IsFalse(string.IsNullOrEmpty(info.GroupTemplateGuid));

            CreateGroupIfNeeded(info.GroupName, info.GroupTemplateGuid);
            CreateOrMoveEntry(info.AssetPath, info.GroupName, info.Address, true, info.Labels ?? new string[0]);
            return true;
        }

        private bool ApplyRemoveInfo(EntryRemoveOperationInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Assert.IsFalse(string.IsNullOrEmpty(info.AssetPath));

            RemoveEntry(info.AssetPath, true);
            return true;
        }

        private bool CreateGroupIfNeeded(string groupName, string groupTemplateGuid)
        {
            Assert.IsFalse(string.IsNullOrEmpty(groupName));
            Assert.IsFalse(string.IsNullOrEmpty(groupTemplateGuid));

            if (_addressablesEditorAdapter.ExistsGroupByName(groupName))
            {
                return false;
            }

            var template = LoadAssetAtGuid<AddressableAssetGroupTemplate>(groupTemplateGuid);
            if (template == null)
            {
                throw new Exception(
                    $"{nameof(AddressableAssetGroupTemplate)}(GUID: {groupTemplateGuid}) is not found.");
            }

            var group = _addressablesEditorAdapter.CreateGroup(groupName, false, false,
                true, null, template.GetTypes());
            // Asset bundles should be packed by group.
            // So, set the BundleMode of the schema for the asset bundle to PackTogether.
            _addressablesEditorAdapter.UpdateGroup(group.Guid, null,
                BundledAssetGroupSchema.BundlePackingMode.PackTogether);
            _assetDatabaseAdapter.SaveAssets();
            return true;
        }

        private void CreateOrMoveEntry(string assetPath, string groupName, string address,
            bool removeBeforeGroupIfEmpty, string[] labels)
        {
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));
            Assert.IsFalse(string.IsNullOrEmpty(groupName));
            Assert.IsFalse(string.IsNullOrEmpty(address));
            Assert.IsTrue(_addressablesEditorAdapter.ExistsGroupByName(groupName));

            var assetGuid = _assetDatabaseAdapter.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(assetGuid))
            {
                throw new Exception($"Asset (Path:{assetPath}) is not found.");
            }

            var beforeGroupGuid = string.Empty;
            foreach (var g in _addressablesEditorAdapter.GetGroupInfos())
            {
                if (_addressablesEditorAdapter.GetEntryInfos(g.Guid).Any(x => x.Guid.Equals(assetGuid)))
                {
                    beforeGroupGuid = g.Guid;
                    break;
                }
            }

            var afterGroup = _addressablesEditorAdapter.GetGroupInfoByName(groupName);

            _addressablesEditorAdapter.CreateOrMoveEntry(assetGuid, afterGroup.Guid);
            _addressablesEditorAdapter.UpdateEntry(assetGuid, address, labels);

            if (!string.IsNullOrEmpty(beforeGroupGuid))
            {
                // Remove the before group if it is empty.
                if (removeBeforeGroupIfEmpty && _addressablesEditorAdapter.GetEntryInfos(beforeGroupGuid).Length == 0)
                {
                    var groupInfo = _addressablesEditorAdapter.GetGroupInfo(beforeGroupGuid);
                    _addressablesEditorAdapter.RemoveGroup(groupInfo.Guid);
                }
            }
        }

        private void RemoveEntry(string assetPath, bool removeGroupIfEmpty)
        {
            var assetGuid = _assetDatabaseAdapter.AssetPathToGUID(assetPath);
            var groupGuid = string.Empty;
            foreach (var g in _addressablesEditorAdapter.GetGroupInfos())
            {
                if (_addressablesEditorAdapter.GetEntryInfos(g.Guid).Any(x => x.Guid.Equals(assetGuid)))
                {
                    groupGuid = g.Guid;
                    break;
                }
            }

            if (string.IsNullOrEmpty(groupGuid))
            {
                return;
            }

            _addressablesEditorAdapter.RemoveAssetEntry(assetGuid);

            // Remove the group if it is empty.
            if (removeGroupIfEmpty && _addressablesEditorAdapter.GetEntryInfos(groupGuid).Length == 0)
            {
                _addressablesEditorAdapter.RemoveGroup(groupGuid);
            }
        }

        private T LoadAssetAtGuid<T>(string guid) where T : Object
        {
            var templateAssetPath = _assetDatabaseAdapter.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(templateAssetPath))
            {
                throw new Exception($"{typeof(T).Name} (GUID:{guid}) is not found.");
            }

            return LoadAssetAtPath<T>(templateAssetPath);
        }

        private T LoadAssetAtPath<T>(string assetPath) where T : Object
        {
            var asset = _assetDatabaseAdapter.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                throw new Exception($"{typeof(T).Name} (Path:{assetPath}) is not found.");
            }

            return asset;
        }
    }
}