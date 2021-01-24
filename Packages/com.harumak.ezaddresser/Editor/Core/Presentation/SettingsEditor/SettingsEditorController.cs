using System;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.UseCase;
using EZAddresser.Editor.Foundation.Observable;
using EZAddresser.Editor.Foundation.Undo;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using SettingsService = EZAddresser.Editor.Core.UseCase.SettingsService;

namespace EZAddresser.Editor.Core.Presentation.SettingsEditor
{
    public class SettingsEditorController : IDisposable
    {
        private readonly AssetProcessService _assetProcessService;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AutoIncrementHistory _history = new AutoIncrementHistory();
        private readonly SettingsService _settingsService;
        private SettingsEditorWindow _window;

        public SettingsEditorController(SettingsService settingsService, AssetProcessService assetProcessService)
        {
            _settingsService = settingsService;
            _assetProcessService = assetProcessService;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Setup(SettingsEditorWindow window)
        {
            _disposables.Clear();
            _window = window;

            _disposables.Add(_window.BasePackingMode.Skip(1).Subscribe(OnBasePackingModeChanged));
            _disposables.Add(_window.BaseAddressingMode.Skip(1).Subscribe(OnBaseAddressingModeChanged));
            _disposables.Add(_window.GroupTemplate.Skip(1).Subscribe(OnGroupTemplateChanged));
            _disposables.Add(_window.OnRedoShortcutExecutedAsObservable.Subscribe(_ => Redo()));
            _disposables.Add(_window.OnUndoShortcutExecutedAsObservable.Subscribe(_ => Undo()));
        }

        private void OnBasePackingModeChanged(PackingMode newValue)
        {
            var oldValue = _settingsService.GetState().BasePackingMode.Value;
            _history.Register($"Update {nameof(_window.BasePackingMode)}",
                () =>
                {
                    _settingsService.UpdateSetting(newValue);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.BasePackingMode)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(oldValue);
                        _settingsService.Save();
                    }
                },
                () =>
                {
                    _settingsService.UpdateSetting(oldValue);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.BasePackingMode)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(newValue);
                        _settingsService.Save();
                    }
                });
        }

        private void OnBaseAddressingModeChanged(AddressingMode newValue)
        {
            var oldValue = _settingsService.GetState().BaseAddressingMode.Value;
            _history.Register($"Update {nameof(_window.BaseAddressingMode)}",
                () =>
                {
                    _settingsService.UpdateSetting(addressingMode: newValue);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.BaseAddressingMode)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(addressingMode: oldValue);
                        _settingsService.Save();
                    }
                },
                () =>
                {
                    _settingsService.UpdateSetting(addressingMode: oldValue);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.BaseAddressingMode)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(addressingMode: newValue);
                        _settingsService.Save();
                    }
                });
        }

        private void OnGroupTemplateChanged(AddressableAssetGroupTemplate newValue)
        {
            var oldGuid = _settingsService.GetState().GroupTemplateGuid.Value;
            _history.Register($"Update {nameof(_window.GroupTemplate)}",
                () =>
                {
                    var assetPath = AssetDatabase.GetAssetPath(newValue);
                    var newGuid = AssetDatabase.AssetPathToGUID(assetPath);
                    _settingsService.UpdateSetting(defaultGroupTemplateGuid: newGuid ?? string.Empty);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.GroupTemplateGuid)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(true);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(defaultGroupTemplateGuid: oldGuid ?? string.Empty);
                        _settingsService.Save();
                    }
                },
                () =>
                {
                    var assetPath = AssetDatabase.GetAssetPath(newValue);
                    var newGuid = AssetDatabase.AssetPathToGUID(assetPath);
                    _settingsService.UpdateSetting(defaultGroupTemplateGuid: oldGuid ?? string.Empty);
                    _settingsService.Save();
                    if (DisplayReprocessConfirmationDialog(nameof(ReadOnlySetting.GroupTemplateGuid)))
                    {
                        _assetProcessService.ReprocessAllAssetsInAddressablesFolder(true);
                    }
                    else
                    {
                        _settingsService.UpdateSetting(defaultGroupTemplateGuid: newGuid ?? string.Empty);
                        _settingsService.Save();
                    }
                });
        }

        private void Redo()
        {
            _history.Redo();
        }

        private void Undo()
        {
            _history.Undo();
        }

        private bool DisplayReprocessConfirmationDialog(string updatePropertyName)
        {
            return EditorUtility.DisplayDialog("Confirmation",
                $"When {updatePropertyName} is updated, it will immediately reprocess all the files in the Addressables folder.",
                "OK", "Cancel");
        }
    }
}