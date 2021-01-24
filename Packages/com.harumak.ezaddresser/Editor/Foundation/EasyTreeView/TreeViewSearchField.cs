using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EZAddresser.Editor.Foundation.EasyTreeView
{
    /// <summary>
    ///     The search field for the Easy Tree View.
    /// </summary>
    internal class TreeViewSearchField
    {
        private readonly string[] _multiHeaderColumnNames;
        private readonly SearchField _searchField;
        private readonly TreeViewBase _treeView;
        private int _selectedColumnNameIndex;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="treeView"></param>
        public TreeViewSearchField(TreeViewBase treeView)
        {
            _searchField = new SearchField();
            _multiHeaderColumnNames =
                treeView.multiColumnHeader?.state.columns.Select(x => x.headerContent.text).ToArray();
            _searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
            _treeView = treeView;
        }

        /// <summary>
        ///     <para>This is the controlID used for the text field to obtain keyboard focus.</para>
        /// </summary>
        public int SearchFieldControlID
        {
            get => _searchField.searchFieldControlID;
            set => _searchField.searchFieldControlID = value;
        }

        /// <summary>
        ///     <para>
        ///         Changes the keyboard focus to the search field when the user presses ‘Ctrl/Cmd + F’ when set to true. It is
        ///         true by default.
        ///     </para>
        /// </summary>
        public bool autoSetFocusOnFindCommand
        {
            get => _searchField.autoSetFocusOnFindCommand;
            set => _searchField.autoSetFocusOnFindCommand = value;
        }

        /// <summary>
        ///     <para>The event that is called when a down/up key is pressed.</para>
        /// </summary>
        public event SearchField.SearchFieldCallback downOrUpArrowKeyPressed
        {
            add => _searchField.downOrUpArrowKeyPressed += value;
            remove => _searchField.downOrUpArrowKeyPressed -= value;
        }

        /// <summary>
        ///     <para>This function changes keyboard focus to the search field so a user can start typing.</para>
        /// </summary>
        public void SetFocus()
        {
            _searchField.SetFocus();
        }

        /// <summary>
        ///     <para>This function returns true if the search field has keyboard focus.</para>
        /// </summary>
        public bool HasFocus()
        {
            return _searchField.HasFocus();
        }

        /// <summary>
        ///     <para>This function displays the search field with a toolbar style in the given Rect.</para>
        /// </summary>
        public void OnToolbarGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, string.Empty, EditorStyles.toolbarButton);
            rect.xMin += 10;
            var searchFieldRect = rect;
            searchFieldRect.y += 2;
            if (_multiHeaderColumnNames != null && _multiHeaderColumnNames.Length >= 1)
            {
                var popupRect = rect;
                popupRect.y += 2;
                popupRect.width = Mathf.Min(popupRect.width, 100);
                searchFieldRect.x += popupRect.width + 9;
                searchFieldRect.width -= popupRect.width + 9;
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var currentColumnName = _multiHeaderColumnNames[_selectedColumnNameIndex];
                    var buttonStyle = new GUIStyle(EditorStyles.popup);
                    buttonStyle.fixedHeight = 16;
                    if (GUI.Button(popupRect, $"Search : {currentColumnName}", buttonStyle))
                    {
                        var menu = new GenericMenu();
                        for (var i = 0; i < _multiHeaderColumnNames.Length; i++)
                        {
                            var columnName = _multiHeaderColumnNames[i];
                            var index = i;
                            menu.AddItem(new GUIContent(columnName), _selectedColumnNameIndex == i, () =>
                            {
                                _selectedColumnNameIndex = index;
                                _treeView.SearchColumnIndex = _selectedColumnNameIndex;
                            });
                        }

                        menu.ShowAsContext();
                    }
                }
            }

            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var searchString = _searchField.OnToolbarGUI(searchFieldRect, _treeView.searchString);
                if (ccs.changed)
                {
                    _treeView.searchString = searchString;
                }
            }
        }

        /// <summary>
        ///     <para>This function displays the search field with a toolbar style.</para>
        /// </summary>
        public void OnToolbarGUI()
        {
            var maxWidth = _multiHeaderColumnNames == null || _multiHeaderColumnNames.Length == 0 ? 200 : 300;
            var rect = GUILayoutUtility.GetRect(100, maxWidth, EditorStyles.toolbar.fixedHeight,
                EditorStyles.toolbar.fixedHeight);
            OnToolbarGUI(rect);
        }
    }
}