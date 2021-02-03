using System;
using System.Collections.Generic;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.UseCase;
using EZAddresser.Editor.Foundation;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable;
using EZAddresser.Editor.Foundation.Undo;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EZAddresser.Editor.Core.Presentation.EntryRulesEditor
{
    public class EntryRulesEditorController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EntryRulesService _entryRulesService;
        private readonly AssetProcessService _assetProcessService;
        private readonly AutoIncrementHistory _history = new AutoIncrementHistory();

        private readonly Dictionary<EntityId, CompositeDisposable> _itemDisposables
            = new Dictionary<EntityId, CompositeDisposable>();

        private EntryRulesEditorTreeView _treeView;
        private EntryRulesEditorWindow _window;

        public EntryRulesEditorController(EntryRulesService entryRulesService, AssetProcessService assetProcessService)
        {
            _entryRulesService = entryRulesService;
            _assetProcessService = assetProcessService;
        }

        public void Dispose()
        {
            DisposeDisposables();
        }

        public void Setup(EntryRulesEditorWindow window)
        {
            ClearDisposables();

            _window = window;
            _treeView = window.TreeView;

            _window.SaveButtonClickedAsObservable.Subscribe(_ => OnSaveButtonClicked()).DisposeWith(_disposables);
            _window.SaveShortcutExecutedAsObservable.Subscribe(_ => OnSaveShortcutExecuted()).DisposeWith(_disposables);
            _window.OnDisableExecuteAsObservable.Subscribe(_ => OnDisableExecuted()).DisposeWith(_disposables);
            _window.UndoShortcutExecutedAsObservable.Subscribe(_ => Undo());
            _window.RedoShortcutExecutedAsObservable.Subscribe(_ => Redo());
            _window.CreateButtonClickedAsObservable.Subscribe(_ => OnCreateButtonClicked()).DisposeWith(_disposables);
            _window.RightClickCreateMenuClickedAsObservable.Subscribe(_ => OnRightClickCreateMenuClicked())
                .DisposeWith(_disposables);
            _window.RightClickRemoveMenuClickedAsObservable.Subscribe(_ => OnRightClickRemoveMenuClicked())
                .DisposeWith(_disposables);
            _treeView.OnItemAddedAsObservable().Subscribe(OnTreeViewItemAdded).DisposeWith(_disposables);
            _treeView.OnItemRemovedAsObservable().Subscribe(OnTreeViewItemRemoved).DisposeWith(_disposables);
            _treeView.OnItemClearedAsObservable().Subscribe(OnTreeViewItemsCleared).DisposeWith(_disposables);
        }

        private void ClearDisposables()
        {
            foreach (var itemDisposable in _itemDisposables.Values)
            {
                itemDisposable.Clear();
            }

            _itemDisposables.Clear();
            _disposables.Clear();
        }

        private void DisposeDisposables()
        {
            foreach (var itemDisposable in _itemDisposables.Values)
            {
                itemDisposable.Dispose();
            }

            _itemDisposables.Clear();
            _disposables.Dispose();
        }

        private void OnSaveButtonClicked()
        {
            if (!_entryRulesService.HasUnsavedChanges.Value)
            {
                return;
            }
            
            if (EditorUtility.DisplayDialog("Confirm", 
                "Do you want to save the entry rules and refresh all entries?", "OK", "Cancel"))
            {
                Save();
                _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
            }
        }

        private void OnSaveShortcutExecuted()
        {
            if (!_entryRulesService.HasUnsavedChanges.Value)
            {
                return;
            }
            
            if (EditorUtility.DisplayDialog("Confirm", 
                "Do you want to save the entry rules and refresh all entries?", "OK", "Cancel"))
            {
                Save();
                _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
            }
        }

        private void OnDisableExecuted()
        {
            if (!_entryRulesService.HasUnsavedChanges.Value)
            {
                return;
            }
            
            if (EditorUtility.DisplayDialog("Confirm", 
                "Some changes have not been saved. Do you want to save and refresh all entries?", "Save", "NOT Save"))
            {
                Save();
                _assetProcessService.ReprocessAllAssetsInAddressablesFolder(false);
            }
        }

        private void OnCreateButtonClicked()
        {
            CreateRule();
        }

        private void OnRightClickCreateMenuClicked()
        {
            CreateRule();
        }

        private void OnRightClickRemoveMenuClicked()
        {
            RemoveSelectedRule();
        }

        private void OnTreeViewItemAdded(TreeViewItem treeViewItem)
        {
            var item = (EntryRulesEditorTreeViewItem) treeViewItem;
            BindTreeViewItemToModel(item);
        }

        private void OnTreeViewItemRemoved(TreeViewItem treeViewItem)
        {
            var item = (EntryRulesEditorTreeViewItem) treeViewItem;
            UnbindTreeViewItemFromModel(item);
        }

        private void OnTreeViewItemsCleared(Empty _)
        {
            UnbindAllTreeViewItemsFromModel();
        }

        private void BindTreeViewItemToModel(EntryRulesEditorTreeViewItem item)
        {
            var disposables = new CompositeDisposable();
            ReadOnlyEntryRule GetRule() => _entryRulesService.GetState().Get(item.ControlId);
            item.AddressablePathRule.Skip(1).Subscribe(x =>
            {
                var newValue = item.AddressablePathRule.Value;
                var oldValue = GetRule().AddressablePathRule.Value;
                _history.Register($"{nameof(OnTreeViewItemAdded)}_{nameof(item.AddressablePathRule)}",
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(newValue)),
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(oldValue)));
            }).DisposeWith(disposables);
            item.AddressingMode.Skip(1).Subscribe(x =>
            {
                var newValue = item.AddressingMode.Value;
                var oldValue = GetRule().AddressingMode.Value;
                _history.Register($"{nameof(OnTreeViewItemAdded)}_{nameof(item.AddressingMode)}",
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(addressingMode: newValue)),
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(addressingMode: oldValue)));
            }).DisposeWith(disposables);
            item.GroupNameRule.Skip(1).Subscribe(x =>
            {
                var newValue = item.GroupNameRule.Value;
                var oldValue = GetRule().GroupNameRule.Value;
                _history.Register($"{nameof(OnTreeViewItemAdded)}_{nameof(item.GroupNameRule)}",
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(groupNameRule: newValue)),
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(groupNameRule: oldValue)));
            }).DisposeWith(disposables);
            item.LabelRules.Skip(1).Subscribe(x =>
            {
                var newValue = item.LabelRules.Value;
                var oldValue = GetRule().LabelRules.Value;
                _history.Register($"{nameof(OnTreeViewItemAdded)}_{nameof(item.LabelRules)}",
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(labelRules: newValue)),
                    () => _entryRulesService.UpdateRule(item.ControlId,
                        new EntryRuleUpdateCommand(labelRules: oldValue)));
            }).DisposeWith(disposables);
            _itemDisposables.Add(item.ControlId, disposables);
        }

        private void UnbindTreeViewItemFromModel(EntryRulesEditorTreeViewItem item)
        {
            var id = item.ControlId;
            _itemDisposables[id].Dispose();
            _itemDisposables.Remove(id);
        }

        private void UnbindAllTreeViewItemsFromModel()
        {
            foreach (var disposable in _itemDisposables.Values)
            {
                disposable.Dispose();
            }

            _itemDisposables.Clear();
        }

        private void Save()
        {
            _entryRulesService.Save();
        }
        
        private void Redo()
        {
            _history.Redo();
            GUI.FocusControl("");
        }

        private void Undo()
        {
            _history.Undo();
            GUI.FocusControl("");
        }

        private void CreateRule()
        {
            _entryRulesService.AddRule();
        }

        private void RemoveSelectedRule()
        {
            if (!_treeView.HasSelection())
            {
                return;
            }

            foreach (var id in _treeView.GetSelection())
            {
                var item = (EntryRulesEditorTreeViewItem) _treeView.GetItem(id);
                _entryRulesService.RemoveRule(item.ControlId);
            }
        }
    }
}