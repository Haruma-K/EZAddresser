using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;

namespace EZAddresser.Editor.Core.UseCase
{
    /// <summary>
    ///     <see cref="AssetPostprocessor" /> for EZAddresser.
    /// </summary>
    public class EZAddresserAssetPostProcessor : AssetPostprocessor
    {
        private const string AddressableAssetSettingsAssetName = "AdressableAssetSettings.asset";

        private static void OnPostprocessAllAssets(string[] importedAssetPaths, string[] deletedAssetPaths,
            string[] movedAssetPaths, string[] movedFromAssetPaths)
        {
            var isSettingsExists = AddressableAssetSettingsDefaultObject.Settings != null;
            var hasSettingsDeleted = deletedAssetPaths.Any(x => x.EndsWith(AddressableAssetSettingsAssetName));
            if (!isSettingsExists || hasSettingsDeleted)
            {
                EditorApplication.delayCall += () =>
                {
                    if (AddressableAssetSettingsDefaultObject.Settings == null)
                    {
                        CompositionRoot.RequestInstance().AssetProcessService
                            .ReprocessAllAssetsInAddressablesFolder(false);
                        CompositionRoot.ReleaseInstance();
                    }
                };
                return;
            }

            var compositionRoot = CompositionRoot.RequestInstance();
            var assetProcessService = compositionRoot.AssetProcessService;

            var processed = false;
            foreach (var importedAssetPath in importedAssetPaths)
            {
                processed |= assetProcessService.ProcessImportedAsset(importedAssetPath);
            }

            foreach (var deletedAssetPath in deletedAssetPaths)
            {
                processed |= assetProcessService.ProcessDeletedAsset(deletedAssetPath);
            }

            for (var i = 0; i < movedAssetPaths.Length; i++)
            {
                var movedToAssetPath = movedAssetPaths[i];
                var movedFromAssetPath = movedFromAssetPaths[i];
                processed |= assetProcessService.ProcessMovedAsset(movedToAssetPath, movedFromAssetPath);
            }

            if (processed)
            {
                //NOTE: If you don't call this when you delete assets other than the asset to be processed in OnPostprocessAllAssets, the AssetDatabase will not be updated properly.
                EditorApplication.delayCall += AssetDatabase.Refresh;
            }

            CompositionRoot.ReleaseInstance();
        }

        public override int GetPostprocessOrder()
        {
            return 1;
        }
    }
}