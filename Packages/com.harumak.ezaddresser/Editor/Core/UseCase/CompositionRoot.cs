using System;
using EZAddresser.Editor.Core.Infrastructure.Adapters;
using EZAddresser.Editor.Core.Infrastructure.Repositories;
using EZAddresser.Editor.Core.Presentation.EntryRulesEditor;
using EZAddresser.Editor.Core.Presentation.SettingsEditor;
using NUnit.Framework;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace EZAddresser.Editor.Core.UseCase
{
    public class CompositionRoot : IDisposable
    {
        private static int _referenceCount;
        private static CompositionRoot _compositionRoot;

        private CompositionRoot()
        {
            // Create AddressableAssetSettings asset if it does not exists.
            if (AddressableAssetSettingsDefaultObject.Settings == null)
            {
                AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(
                    AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                    AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);
            }

            var settingsRepository = new SettingsRepository();
            var entryRulesRepository = new EntryRulesRepository();

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var addressablesEditorAdapter = new AddressablesEditorAdapter(addressableSettings);
            var assetDatabaseAdapter = new AssetDatabaseAdapter();
            AssetProcessService = new AssetProcessService(settingsRepository, entryRulesRepository,
                assetDatabaseAdapter,
                addressablesEditorAdapter);
            SettingsService = new SettingsService(settingsRepository);
            EntryRulesService = new EntryRulesService(entryRulesRepository);
            SettingsEditorPresenter = new SettingsEditorPresenter(SettingsService);
            SettingsEditorController = new SettingsEditorController(SettingsService, AssetProcessService);
            EntryRulesEditorPresenter = new EntryRulesEditorPresenter(EntryRulesService);
            EntryRulesEditorController = new EntryRulesEditorController(EntryRulesService, AssetProcessService);
        }

        public SettingsService SettingsService { get; }
        public EntryRulesService EntryRulesService { get; }
        public AssetProcessService AssetProcessService { get; }

        public SettingsEditorPresenter SettingsEditorPresenter { get; }
        public SettingsEditorController SettingsEditorController { get; }
        public EntryRulesEditorPresenter EntryRulesEditorPresenter { get; }
        public EntryRulesEditorController EntryRulesEditorController { get; }

        public void Dispose()
        {
            SettingsEditorPresenter.Dispose();
            SettingsEditorController.Dispose();
            EntryRulesEditorPresenter.Dispose();
            EntryRulesEditorController.Dispose();
        }

        public static CompositionRoot RequestInstance()
        {
            if (_referenceCount++ == 0)
            {
                _compositionRoot = new CompositionRoot();
            }

            return _compositionRoot;
        }

        public static void ReleaseInstance()
        {
            if (--_referenceCount == 0)
            {
                _compositionRoot.Dispose();
                _compositionRoot = null;
            }

            Assert.IsTrue(_referenceCount >= 0);
        }
    }
}