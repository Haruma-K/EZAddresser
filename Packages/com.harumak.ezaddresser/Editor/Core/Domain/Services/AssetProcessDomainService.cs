using System;
using System.Collections.Generic;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryOperationInfos;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using UnityEditor.AddressableAssets.Settings;

namespace EZAddresser.Editor.Core.Domain.Services
{
    /// <summary>
    ///     Service that processes assets.
    /// </summary>
    public class AssetProcessDomainService
    {
        private readonly AddressablePathGenerateDomainService _addressablePathGenerateService;
        private readonly AddressableAssetSettings _addressableSettings;
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;
        private readonly EntryOperationInfoApplyDomainService _entryOperationInfoApplyService;
        private readonly EntryOperationInfoBuildDomainService _entryOperationInfoBuildService;
        private readonly IEntryRulesRepository _rulesRepository;
        private readonly ISettingsRepository _settingsRepository;
        private EntryRuleSet _rules;
        private Setting _setting;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="settingsRepository"></param>
        /// <param name="rulesRepository"></param>
        /// <param name="addressableSettings"></param>
        /// <param name="assetDatabaseAdapter"></param>
        /// <param name="entryOperationInfoBuildService"></param>
        /// <param name="entryOperationInfoApplyService"></param>
        /// <param name="addressablePathGenerateService"></param>
        public AssetProcessDomainService(ISettingsRepository settingsRepository, IEntryRulesRepository rulesRepository,
            AddressableAssetSettings addressableSettings,
            IAssetDatabaseAdapter assetDatabaseAdapter,
            EntryOperationInfoBuildDomainService entryOperationInfoBuildService,
            EntryOperationInfoApplyDomainService entryOperationInfoApplyService,
            AddressablePathGenerateDomainService addressablePathGenerateService)
        {
            _settingsRepository = settingsRepository;
            _rulesRepository = rulesRepository;
            _addressableSettings = addressableSettings;
            _assetDatabaseAdapter = assetDatabaseAdapter;
            _entryOperationInfoBuildService = entryOperationInfoBuildService;
            _entryOperationInfoApplyService = entryOperationInfoApplyService;
            _addressablePathGenerateService = addressablePathGenerateService;
        }

        /// <summary>
        ///     Call this before you process.
        /// </summary>
        public void Setup()
        {
            _setting = _settingsRepository.Fetch();
            _rules = _rulesRepository.Fetch();
        }

        /// <summary>
        ///     Reprocesses all assets stored in the Addressables folder.
        /// </summary>
        /// <param name="clean">Remove all entries before reprocessing.</param>
        /// <returns>True if processed.</returns>
        public bool ReprocessAllAssetsInAddressablesFolder(bool clean)
        {
            SetDefaultGroupTemplateIfNeeded();

            var processed = false;
            if (clean)
            {
                var assetPathsInAddressablesFolder = new List<string>();
                foreach (var assetPath in _assetDatabaseAdapter.GetAllAssetPaths())
                {
                    var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(assetPath);
                    if (!string.IsNullOrEmpty(addressablePath))
                    {
                        processed |= ProcessDeletedAsset(assetPath);
                        assetPathsInAddressablesFolder.Add(assetPath);
                    }
                }

                foreach (var assetPath in assetPathsInAddressablesFolder)
                {
                    processed |= ProcessImportedAsset(assetPath);
                }
            }
            else
            {
                foreach (var assetPath in _assetDatabaseAdapter.GetAllAssetPaths())
                {
                    var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(assetPath);
                    if (!string.IsNullOrEmpty(addressablePath))
                    {
                        processed |= ProcessImportedAsset(assetPath);
                    }
                }
            }

            return processed;
        }

        /// <summary>
        ///     Process a imported asset.
        /// </summary>
        /// <param name="importedAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessImportedAsset(string importedAssetPath)
        {
            SetDefaultGroupTemplateIfNeeded();

            var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(importedAssetPath);
            if (string.IsNullOrEmpty(addressablePath))
            {
                return false;
            }

            var addressingMode = _setting.BaseAddressingMode.Value;
            var packingMode = _setting.BasePackingMode.Value;
            var groupTemplateGuid = _setting.GroupTemplateGuid.Value;
            var createOrMoveInfo = _entryOperationInfoBuildService.BuildEntryCreateOrMoveInfo(importedAssetPath,
                addressingMode, packingMode, groupTemplateGuid, _rules);
            if (createOrMoveInfo == null)
            {
                return false;
            }

            var operationInfo = new EntryOperationInfo(createOrMoveInfo, null);
            return _entryOperationInfoApplyService.Apply(operationInfo);
        }

        /// <summary>
        ///     Process a deleted asset.
        /// </summary>
        /// <param name="deletedAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessDeletedAsset(string deletedAssetPath)
        {
            SetDefaultGroupTemplateIfNeeded();

            var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(deletedAssetPath);
            if (string.IsNullOrEmpty(addressablePath))
            {
                return false;
            }

            var removeInfo = _entryOperationInfoBuildService.BuildEntryRemoveInfo(deletedAssetPath);
            if (removeInfo == null)
            {
                return false;
            }

            var operationInfo = new EntryOperationInfo(null, removeInfo);
            return _entryOperationInfoApplyService.Apply(operationInfo);
        }

        /// <summary>
        ///     Process a moved asset.
        /// </summary>
        /// <param name="toAssetPath"></param>
        /// <param name="fromAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessMovedAsset(string toAssetPath, string fromAssetPath)
        {
            SetDefaultGroupTemplateIfNeeded();

            var toAddressablePath = _addressablePathGenerateService.GenerateFromAssetPath(toAssetPath);
            var fromAddressablePath = _addressablePathGenerateService.GenerateFromAssetPath(fromAssetPath);

            var addressingMode = _setting.BaseAddressingMode.Value;
            var packingMode = _setting.BasePackingMode.Value;
            var groupTemplateGuid = _setting.GroupTemplateGuid.Value;

            if (!string.IsNullOrEmpty(toAddressablePath) && !string.IsNullOrEmpty(fromAddressablePath))
            {
                // Moving from the Addressables folder to the Addressables folder.
                // So, move the entry.
                var createOrMoveInfo = _entryOperationInfoBuildService.BuildEntryCreateOrMoveInfo(toAssetPath,
                    addressingMode, packingMode, groupTemplateGuid, _rules);
                if (createOrMoveInfo == null)
                {
                    return false;
                }

                var operationInfo = new EntryOperationInfo(createOrMoveInfo, null);
                return _entryOperationInfoApplyService.Apply(operationInfo);
            }

            if (!string.IsNullOrEmpty(toAddressablePath) && string.IsNullOrEmpty(fromAddressablePath))
            {
                // Moving from the non-Addressables folder to the Addressables folder.
                // So, create the entry.
                var createOrMoveInfo = _entryOperationInfoBuildService.BuildEntryCreateOrMoveInfo(toAssetPath,
                    addressingMode, packingMode, groupTemplateGuid, _rules);
                if (createOrMoveInfo == null)
                {
                    return false;
                }

                var operationInfo = new EntryOperationInfo(createOrMoveInfo, null);
                return _entryOperationInfoApplyService.Apply(operationInfo);
            }

            if (string.IsNullOrEmpty(toAddressablePath) && !string.IsNullOrEmpty(fromAddressablePath))
            {
                // Moving from the Addressables folder to the non-Addressables folder.
                // So, remove the entry.
                var removeInfo = _entryOperationInfoBuildService.BuildEntryRemoveInfo(toAssetPath);
                if (removeInfo == null)
                {
                    return false;
                }

                var operationInfo = new EntryOperationInfo(null, removeInfo);
                return _entryOperationInfoApplyService.Apply(operationInfo);
            }

            // Moving from the non-Addressables folder to the non-Addressables folder.
            // So, do nothing.
            return false;
        }

        private void SetDefaultGroupTemplateIfNeeded()
        {
            if (string.IsNullOrEmpty(_setting.GroupTemplateGuid.Value)
                || string.IsNullOrEmpty(_assetDatabaseAdapter.GUIDToAssetPath(_setting.GroupTemplateGuid.Value)))
            {
                if (_addressableSettings.GroupTemplateObjects.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"There is no valid {nameof(AddressableAssetGroupTemplate)} in {nameof(AddressableAssetSettings)}.");
                }

                var templateAsset = _addressableSettings.GroupTemplateObjects[0];
                var templateAssetPath = _assetDatabaseAdapter.GetAssetPath(templateAsset);
                var templateAssetGuid = _assetDatabaseAdapter.AssetPathToGUID(templateAssetPath);
                _setting.SetGroupTemplateGuid(templateAssetGuid);
            }
        }
    }
}