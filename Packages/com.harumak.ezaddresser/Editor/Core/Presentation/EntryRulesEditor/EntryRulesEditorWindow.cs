using System;
using EZAddresser.Editor.Core.UseCase;
using EZAddresser.Editor.Foundation.EasyTreeView;
using EZAddresser.Editor.Foundation.Observable;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EZAddresser.Editor.Core.Presentation.EntryRulesEditor
{
    public class EntryRulesEditorWindow : EditorWindow
    {
        public const string Title = "Entry Rule Editor";
        [SerializeField] private TreeViewState _treeViewState;
        private readonly Subject<Empty> _onDisableExecuteSubject = new Subject<Empty>();
        private readonly Subject<Empty> _createButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _redoShortcutExecutedSubject = new Subject<Empty>();

        private readonly Subject<Empty> _rightClickCreateMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickRemoveMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _saveButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _saveShortcutExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _undoShortcutExecutedSubject = new Subject<Empty>();

        private CompositionRoot _compositionRoot;
        private EntryRulesEditorController _controller;
        private EntryRulesEditorPresenter _presenter;
        private TreeViewSearchField _searchField;

        internal EntryRulesEditorTreeView TreeView { get; private set; }

        public bool SaveButtonEnabled { get; set; }
        public IObservable<Empty> RightClickCreateMenuClickedAsObservable => _rightClickCreateMenuClickedSubject;
        public IObservable<Empty> RightClickRemoveMenuClickedAsObservable => _rightClickRemoveMenuClickedSubject;
        public IObservable<Empty> OnDisableExecuteAsObservable => _onDisableExecuteSubject;
        public IObservable<Empty> SaveShortcutExecutedAsObservable => _saveShortcutExecutedSubject;
        public IObservable<Empty> UndoShortcutExecutedAsObservable => _undoShortcutExecutedSubject;
        public IObservable<Empty> RedoShortcutExecutedAsObservable => _redoShortcutExecutedSubject;
        public IObservable<Empty> CreateButtonClickedAsObservable => _createButtonClickedSubject;
        public IObservable<Empty> SaveButtonClickedAsObservable => _saveButtonClickedSubject;

        private void OnEnable()
        {
            if (_treeViewState == null)
            {
                _treeViewState = new TreeViewState();
            }

            TreeView = new EntryRulesEditorTreeView(_treeViewState);
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create New"), false,
                () => _rightClickCreateMenuClickedSubject.OnNext(Empty.Default));
            menu.AddItem(new GUIContent("Remove Selected"), false,
                () => _rightClickRemoveMenuClickedSubject.OnNext(Empty.Default));
            menu.ShowAsContext();
            TreeView.RightClickMenu = menu;
            _searchField = new TreeViewSearchField(TreeView);

            _compositionRoot = CompositionRoot.RequestInstance();
            _controller = _compositionRoot.EntryRulesEditorController;
            _controller.Setup(this);
            _presenter = _compositionRoot.EntryRulesEditorPresenter;
            _presenter.Setup(this);
        }

        private void OnDisable()
        {
            _onDisableExecuteSubject.OnNext(Empty.Default);
            CompositionRoot.ReleaseInstance();
        }

        private void OnGUI()
        {
            // Shortcut
            var e = Event.current;
            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.S)
            {
                _saveShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
            {
                _undoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Y)
            {
                _redoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            // SearchField
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Create", EditorStyles.toolbarDropDown, GUILayout.Width(80)))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Create New"), false,
                        () => _createButtonClickedSubject.OnNext(Empty.Default));
                    menu.ShowAsContext();
                }

                GUILayout.Space(100);
                GUILayout.FlexibleSpace();
                _searchField.OnToolbarGUI();
                GUI.enabled = SaveButtonEnabled;
                if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    _saveButtonClickedSubject?.OnNext(Empty.Default);
                }

                GUI.enabled = true;
            }

            // TreeView
            var toolbarHeight = EditorStyles.toolbar.fixedHeight;
            var treeViewRect = EditorGUILayout.GetControlRect(false);
            treeViewRect.height = position.height - toolbarHeight;
            TreeView.OnGUI(treeViewRect);
        }

        [MenuItem("Window/EZAddresser/Entry Rule Editor")]
        public static void Open()
        {
            GetWindow<EntryRulesEditorWindow>(Title);
        }

        public void SetTitleState(bool isDirty)
        {
            var currentTitle = Title;
            if (isDirty)
            {
                currentTitle = $"* {Title}";
            }

            titleContent = new GUIContent(currentTitle);
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