using System;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.UseCase;
using EZAddresser.Editor.Foundation.Observable;
using EZAddresser.Editor.Foundation.Observable.InteractiveProperty;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace EZAddresser.Editor.Core.Presentation.SettingsEditor
{
    public class SettingsEditorWindow : EditorWindow
    {
        public const string Title = "EZAddresser Settings";

        private readonly InteractiveProperty<AddressingMode>
            _baseAddressingMode = new InteractiveProperty<AddressingMode>();

        private readonly InteractiveProperty<AddressableAssetGroupTemplate> _groupTemplate =
            new InteractiveProperty<AddressableAssetGroupTemplate>();

        private readonly InteractiveProperty<PackingMode> _basePackingMode = new InteractiveProperty<PackingMode>();

        private readonly Subject<Empty> _onCloseButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _onRedoShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _onUndoShortcutExecutedSubject = new Subject<Empty>();

        private CompositionRoot _compositionRoot;

        public IObservable<Empty> OnRedoShortcutExecutedAsObservable => _onRedoShortcutExecutedSubject;
        public IObservable<Empty> OnUndoShortcutExecutedAsObservable => _onUndoShortcutExecutedSubject;
        public ISetOnlyInteractiveProperty<PackingMode> BasePackingMode => _basePackingMode;
        public ISetOnlyInteractiveProperty<AddressingMode> BaseAddressingMode => _baseAddressingMode;
        public ISetOnlyInteractiveProperty<AddressableAssetGroupTemplate> GroupTemplate => _groupTemplate;

        private void OnEnable()
        {
            _compositionRoot = CompositionRoot.RequestInstance();
            var presenter = _compositionRoot.SettingsEditorPresenter;
            presenter.Setup(this);
            var controller = _compositionRoot.SettingsEditorController;
            controller.Setup(this);
        }

        private void OnDisable()
        {
            _onCloseButtonClickedSubject.OnNext(Empty.Default);
            CompositionRoot.ReleaseInstance();
        }

        private void OnGUI()
        {
            DrawGUI();
            switch (Event.current.type)
            {
                case EventType.KeyDown:
                    ProcessKeyDown();
                    return;
            }
        }

        private void DrawGUI()
        {
            _basePackingMode.Value =
                (PackingMode) EditorGUILayout.EnumPopup("Base Packing Mode", _basePackingMode.Value);
            _baseAddressingMode.Value =
                (AddressingMode) EditorGUILayout.EnumPopup("Base Addressing Mode", _baseAddressingMode.Value);
            _groupTemplate.Value =
                (AddressableAssetGroupTemplate) EditorGUILayout.ObjectField("Group Template",
                    _groupTemplate.Value, typeof(AddressableAssetGroupTemplate), false);
        }

        private void ProcessKeyDown()
        {
            // Shortcut
            var e = Event.current;
            if (GetEventAction(e) && e.keyCode == KeyCode.Z)
            {
                _onUndoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.keyCode == KeyCode.Y)
            {
                _onRedoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }
        }

        [MenuItem("Window/EZAddresser/Settings")]
        public static void Open()
        {
            GetWindow<SettingsEditorWindow>(Title);
        }

        private bool GetEventAction(Event e)
        {
#if UNITY_EDITOR_WIN
            return e.control;
#else
            return e.command;
#endif
        }
    }
}