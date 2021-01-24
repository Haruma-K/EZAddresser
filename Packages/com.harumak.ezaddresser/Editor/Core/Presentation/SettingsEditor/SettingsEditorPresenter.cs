using System;
using EZAddresser.Editor.Foundation.Observable;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using SettingsService = EZAddresser.Editor.Core.UseCase.SettingsService;

namespace EZAddresser.Editor.Core.Presentation.SettingsEditor
{
    public class SettingsEditorPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly SettingsService _settingsService;
        private SettingsEditorWindow _window;

        public SettingsEditorPresenter(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Setup(SettingsEditorWindow window)
        {
            _disposables.Clear();
            _window = window;

            var state = _settingsService.GetState();
            _disposables.Add(state.BasePackingMode.Subscribe(x => _window.BasePackingMode.SetValueAndNotNotify(x)));
            _disposables.Add(
                state.BaseAddressingMode.Subscribe(x => _window.BaseAddressingMode.SetValueAndNotNotify(x)));
            _disposables.Add(state.GroupTemplateGuid.Subscribe(x =>
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(x);
                var groupTemplate = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupTemplate>(assetPath);
                _window.GroupTemplate.SetValueAndNotNotify(groupTemplate);
            }));
        }
    }
}