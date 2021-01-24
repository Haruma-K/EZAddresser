using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.Domain.Services;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace EZAddresser.Editor.Core.UseCase
{
    public class AssetProcessService
    {
        private readonly AssetProcessDomainService _domainService;

        public AssetProcessService(ISettingsRepository settingsRepository, IEntryRulesRepository entryRulesRepository,
            IAssetDatabaseAdapter assetDatabaseAdapter, IAddressablesEditorAdapter addressablesEditorAdapter)
        {
            // Create AddressableAssetSettings asset if it does not exists.
            if (AddressableAssetSettingsDefaultObject.Settings == null)
            {
                AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(
                    AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                    AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
            }

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var addressablePathGenerateService = new AddressablePathGenerateDomainService();
            var addressGenerateService = new AddressGenerateDomainService(addressablePathGenerateService);
            var groupNameService = new GroupNameGenerateDomainService(assetDatabaseAdapter);
            var entryOperationInfoBuildService =
                new EntryOperationInfoBuildDomainService(addressablePathGenerateService, addressGenerateService,
                    groupNameService, assetDatabaseAdapter);
            var entryOperationInfoApplyService =
                new EntryOperationInfoApplyDomainService(addressablesEditorAdapter, assetDatabaseAdapter);
            _domainService = new AssetProcessDomainService(settingsRepository, entryRulesRepository,
                addressableSettings, assetDatabaseAdapter, entryOperationInfoBuildService,
                entryOperationInfoApplyService, addressablePathGenerateService);
        }

        /// <summary>
        ///     Reprocesses all assets stored in the Addressables folder.
        /// </summary>
        /// <param name="clean">Remove all entries before reprocessing.</param>
        /// <returns>True if processed.</returns>
        public bool ReprocessAllAssetsInAddressablesFolder(bool clean)
        {
            _domainService.Setup();
            return _domainService.ReprocessAllAssetsInAddressablesFolder(clean);
        }

        /// <summary>
        ///     Process a imported asset.
        /// </summary>
        /// <param name="importedAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessImportedAsset(string importedAssetPath)
        {
            _domainService.Setup();
            return _domainService.ProcessImportedAsset(importedAssetPath);
        }

        /// <summary>
        ///     Process a deleted asset.
        /// </summary>
        /// <param name="deletedAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessDeletedAsset(string deletedAssetPath)
        {
            _domainService.Setup();
            return _domainService.ProcessDeletedAsset(deletedAssetPath);
        }

        /// <summary>
        ///     Process a moved asset.
        /// </summary>
        /// <param name="toAssetPath"></param>
        /// <param name="fromAssetPath"></param>
        /// <returns>True if processed.</returns>
        public bool ProcessMovedAsset(string toAssetPath, string fromAssetPath)
        {
            _domainService.Setup();
            return _domainService.ProcessMovedAsset(toAssetPath, fromAssetPath);
        }
    }
}