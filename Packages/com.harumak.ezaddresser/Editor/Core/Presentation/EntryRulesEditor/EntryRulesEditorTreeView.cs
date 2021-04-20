using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Foundation;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.EasyTreeView;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EZAddresser.Editor.Core.Presentation.EntryRulesEditor
{
    internal sealed class EntryRulesEditorTreeView : TreeViewBase
    {
        public EntryRulesEditorTreeView(TreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            rowHeight = EditorGUIUtility.singleLineHeight + 8;
            Reload();
        }

        protected override MultiColumnHeaderState.Column[] ColumnStates
        {
            get
            {
                var addressablePathRule = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Addressable Path Rule"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 300,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = true
                };
                var addressingModeColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Addressing Mode"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 150,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = true
                };
                var groupNameRuleColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Group Name Rule"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 150,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = true
                };
                var labelRulesColumn = new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Label Rule(s)"),
                    headerTextAlignment = TextAlignment.Center,
                    canSort = true,
                    width = 150,
                    minWidth = 50,
                    autoResize = false,
                    allowToggleVisibility = true
                };
                return new[] {addressablePathRule, addressingModeColumn, groupNameRuleColumn, labelRulesColumn};
            }
        }

        public SetOnlyEntryRulesEditorTreeViewItem AddItem(EntityId entityId, string addressablePathRule,
            AddressingMode addressingMode, string groupNameRule, string labelRules)
        {
            
            var item = new EntryRulesEditorTreeViewItem(entityId)
            {
                id = EntityIdToIntId(entityId),
                displayName = addressablePathRule
            };
            item.AddressablePathRule.Value = addressablePathRule;
            item.AddressingMode.Value = addressingMode;
            item.GroupNameRule.Value = groupNameRule;
            item.LabelRules.Value = labelRules;
            AddItemAndSetParent(item, -1);
            return new SetOnlyEntryRulesEditorTreeViewItem(item);
        }

        public void RemoveItem(EntityId entityId, bool invokeCallback = true)
        {
            base.RemoveItem(EntityIdToIntId(entityId), invokeCallback);
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            cellRect.height -= 4;
            cellRect.y += 2;
            var item = (EntryRulesEditorTreeViewItem) args.item;
            switch ((Columns) columnIndex)
            {
                case Columns.AddressablePathRule:
                    using (new GUIColorOverride(item.IsAddressablePathRuleValid ? GUI.color : Color.red))
                    {
                        item.AddressablePathRule.Value = EditorGUI.TextField(cellRect, item.AddressablePathRule.Value);
                    }

                    break;
                case Columns.AddressingMode:
                    cellRect.y += 2;
                    item.AddressingMode.Value =
                        (AddressingMode) EditorGUI.EnumPopup(cellRect, item.AddressingMode.Value);
                    break;
                case Columns.GroupNameRule:
                    using (new GUIColorOverride(item.IsGroupNameValid ? GUI.color : Color.red))
                    {
                        item.GroupNameRule.Value = EditorGUI.TextField(cellRect, item.GroupNameRule.Value);
                    }

                    break;
                case Columns.LabelRules:
                    using (new GUIColorOverride(item.IsLabelRulesValid ? GUI.color : Color.red))
                    {
                        item.LabelRules.Value = EditorGUI.TextField(cellRect, item.LabelRules.Value);
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            string KeySelector(TreeViewItem x)
            {
                return GetText((EntryRulesEditorTreeViewItem) x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((EntryRulesEditorTreeViewItem) item, columnIndex);
        }

        private static string GetText(EntryRulesEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns) columnIndex)
            {
                case Columns.AddressablePathRule:
                    return item.AddressablePathRule.Value;
                case Columns.AddressingMode:
                    return item.AddressingMode.Value.ToString();
                case Columns.GroupNameRule:
                    return item.GroupNameRule.Value;
                case Columns.LabelRules:
                    return item.LabelRules.Value;
                default:
                    throw new NotImplementedException();
            }
        }

        private static int EntityIdToIntId(EntityId entityId)
        {
            var csp = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(entityId.Value);
            var hash = csp.ComputeHash(bytes);
            var id = BitConverter.ToInt32(hash, 0);
            return id;
        }

        private enum Columns
        {
            AddressablePathRule,
            AddressingMode,
            GroupNameRule,
            LabelRules
        }
    }
}