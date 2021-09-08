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
        private static AddressableAssetSettings _addressableSettings;

        private CompositionRoot()
        {
            Refresh();
        }

        public SettingsService SettingsService { get; private set; }
        public EntryRulesService EntryRulesService { get; private set; }
        public AssetProcessService AssetProcessService { get; private set; }

        public SettingsEditorPresenter SettingsEditorPresenter { get; private set; }
        public SettingsEditorController SettingsEditorController { get; private set; }
        public EntryRulesEditorPresenter EntryRulesEditorPresenter { get; private set; }
        public EntryRulesEditorController EntryRulesEditorController { get; private set; }

        public void Dispose()
        {
            SettingsEditorPresenter.Dispose();
            SettingsEditorController.Dispose();
            EntryRulesEditorPresenter.Dispose();
            EntryRulesEditorController.Dispose();
        }

        public static CompositionRoot RequestInstance()
        {
            if (_referenceCount++ == 0 || _compositionRoot == null)
            {
                _compositionRoot = new CompositionRoot();
            }

            if (!_compositionRoot.IsValid())
            {
                _compositionRoot.Refresh();
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

        private bool IsValid()
        {
            return _addressableSettings != null;
        }

        private void Refresh()
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

            _addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var addressablesEditorAdapter = new AddressablesEditorAdapter(_addressableSettings);
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
    }
}