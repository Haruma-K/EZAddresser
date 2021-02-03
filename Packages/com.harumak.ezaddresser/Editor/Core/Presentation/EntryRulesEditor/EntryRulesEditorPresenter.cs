using System;
using System.Collections.Generic;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.UseCase;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

namespace EZAddresser.Editor.Core.Presentation.EntryRulesEditor
{
    public class EntryRulesEditorPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EntryRulesService _entryRulesService;

        private readonly Dictionary<EntityId, CompositeDisposable> _perItemDisposables =
            new Dictionary<EntityId, CompositeDisposable>();

        private EntryRulesEditorTreeView _treeView;
        private EntryRulesEditorWindow _window;

        public EntryRulesEditorPresenter(EntryRulesService entryRulesService)
        {
            _entryRulesService = entryRulesService;
        }

        public void Dispose()
        {
            DisposeDisposables();
        }

        private void ClearDisposables()
        {
            foreach (var disposable in _perItemDisposables.Values)
            {
                disposable.Clear();
            }

            _perItemDisposables.Clear();
            _disposables.Clear();
        }

        private void DisposeDisposables()
        {
            foreach (var disposable in _perItemDisposables.Values)
            {
                disposable.Dispose();
            }

            _perItemDisposables.Clear();
            _disposables.Dispose();
        }

        public void Setup(EntryRulesEditorWindow window)
        {
            ClearDisposables();

            _window = window;
            _treeView = window.TreeView;

            _entryRulesService.HasUnsavedChanges.Subscribe(OnUnsavedFlagChanged).DisposeWith(_disposables);
            var rules = _entryRulesService.GetState();
            rules.ObservableAdd.Subscribe(x => OnRuleAdded(x.Value)).DisposeWith(_disposables);
            rules.ObservableRemove.Subscribe(x => OnRuleRemoved(x.Value)).DisposeWith(_disposables);
            rules.ObservableClear.Subscribe(x => OnRuleCleared()).DisposeWith(_disposables);

            foreach (var rule in rules)
            {
                AddTreeViewItem(rule);
            }
        }

        private void OnUnsavedFlagChanged(bool hasUnsavedChanges)
        {
            _window.SaveButtonEnabled = hasUnsavedChanges;
            _window.SetTitleState(hasUnsavedChanges);
        }

        private void OnRuleAdded(ReadOnlyEntryRule rule)
        {
            AddTreeViewItem(rule);
        }

        private void OnRuleRemoved(ReadOnlyEntryRule rule)
        {
            RemoveTreeViewItem(rule);
        }

        private void OnRuleCleared()
        {
            ClearTreeViewItem();
        }

        private void AddTreeViewItem(ReadOnlyEntryRule rule)
        {
            var viewItem = _treeView.AddItem(rule.Id, rule.AddressablePathRule.Value, rule.AddressingMode.Value,
                rule.GroupNameRule.Value, rule.LabelRules.Value);
            _treeView.Reload();

            var itemDisposables = new CompositeDisposable();
            rule.AddressablePathRule.Subscribe(x =>
                {
                    viewItem.AddressablePathRule.SetValueAndNotNotify(x);
                    viewItem.IsAddressablePathRuleValid = rule.ValidateAddressablePathRule(out _);
                })
                .DisposeWith(itemDisposables);
            rule.AddressingMode.Subscribe(x => viewItem.AddressingMode.SetValueAndNotNotify(x))
                .DisposeWith(itemDisposables);
            rule.GroupNameRule.Subscribe(x =>
                {
                    viewItem.GroupNameRule.SetValueAndNotNotify(x);
                    viewItem.IsGroupNameValid = rule.ValidateGroupNameRule(out _);
                })
                .DisposeWith(itemDisposables);
            rule.LabelRules.Subscribe(x =>
                {
                    viewItem.LabelRules.SetValueAndNotNotify(x);
                    viewItem.IsLabelRulesValid = rule.ValidateLabelRules(out _);
                })
                .DisposeWith(itemDisposables);
            _perItemDisposables.Add(rule.Id, itemDisposables);
        }

        private void RemoveTreeViewItem(ReadOnlyEntryRule rule)
        {
            _perItemDisposables[rule.Id].Dispose();

            _treeView.RemoveItem(rule.Id);
            _treeView.SetSelection(new List<int>());
            _treeView.Reload();
        }

        private void ClearTreeViewItem()
        {
            foreach (var disposable in _perItemDisposables.Values)
            {
                disposable.Clear();
            }

            _treeView.ClearItems();
            _treeView.Reload();
        }
    }
}