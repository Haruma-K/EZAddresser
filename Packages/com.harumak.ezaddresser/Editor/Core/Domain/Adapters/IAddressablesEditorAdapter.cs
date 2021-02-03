using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace EZAddresser.Editor.Core.Domain.Adapters
{
    /// <summary>
    ///     Interface for editing Addressables assets.
    /// </summary>
    public interface IAddressablesEditorAdapter
    {
        /// <summary>
        ///     Get a group info.
        /// </summary>
        /// <returns></returns>
        AddressableAssetGroupInfo GetGroupInfo(string groupGuid);

        /// <summary>
        ///     Get a group info by name.
        /// </summary>
        /// <returns></returns>
        AddressableAssetGroupInfo GetGroupInfoByName(string groupName);

        /// <summary>
        ///     Get all group infos.
        /// </summary>
        /// <returns></returns>
        AddressableAssetGroupInfo[] GetGroupInfos();

        /// <summary>
        ///     Returns true if the group exists.
        /// </summary>
        /// <param name="groupGuid">The name of the group.</param>
        /// <returns>The group found or null.</returns>
        bool ExistsGroup(string groupGuid);

        /// <summary>
        ///     Returns true if the group exists.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <returns>The group found or null.</returns>
        bool ExistsGroupByName(string groupName);

        /// <summary>
        ///     Create a new asset group.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <param name="setAsDefaultGroup">Set the new group as the default group.</param>
        /// <param name="readOnly">Is the new group read only.</param>
        /// <param name="postEvent">Post modification event.</param>
        /// <param name="schemasToCopy">Schema set to copy from.</param>
        /// <param name="types">Types of schemas to add.</param>
        AddressableAssetGroupInfo CreateGroup(string groupName, bool setAsDefaultGroup, bool readOnly, bool postEvent,
            List<AddressableAssetGroupSchema> schemasToCopy, params Type[] types);

        /// <summary>
        ///     Update a group.
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <param name="name"></param>
        /// <param name="bundlePackingMode"></param>
        AddressableAssetGroupInfo UpdateGroup(string groupGuid, string name = null,
            BundledAssetGroupSchema.BundlePackingMode? bundlePackingMode = null);

        /// <summary>
        ///     Remove an asset group.
        /// </summary>
        /// <param name="groupGuid"></param>
        void RemoveGroup(string groupGuid);

        /// <summary>
        ///     Get the paths of all assets included in the group.
        /// </summary>
        /// <param name="groupGuid"></param>
        /// <returns></returns>
        AddressableAssetEntryInfo[] GetEntryInfos(string groupGuid);

        /// <summary>
        ///     Create a new entry, or if one exists in a different group, move it into the new group.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="groupGuid">The group to add the entry to.</param>
        /// <param name="readOnly">Is the new entry read only.</param>
        /// <param name="postEvent">Send modification event.</param>
        AddressableAssetEntryInfo CreateOrMoveEntry(string assetGuid, string groupGuid, bool readOnly = false,
            bool postEvent = true);

        /// <summary>
        ///     Update a entry.
        /// </summary>
        /// <param name="assetGuid"></param>
        /// <param name="address"></param>
        /// <param name="labels"></param>
        void UpdateEntry(string assetGuid, string address = null, string[] labels = null);

        /// <summary>
        ///     Remove an asset entry.
        /// </summary>
        /// <param name="assetGuid">The  guid of the asset.</param>
        /// <param name="postEvent">Send modification event.</param>
        void RemoveAssetEntry(string assetGuid, bool postEvent = true);
    }
}