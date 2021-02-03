using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.InteractiveProperty;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EZAddresser.Editor.Core.Presentation.EntryRulesEditor
{
    public class EntryRulesEditorTreeViewItem : TreeViewItem
    {
        public readonly InteractiveProperty<string> AddressablePathRule = new InteractiveProperty<string>();
        public readonly InteractiveProperty<AddressingMode> AddressingMode = new InteractiveProperty<AddressingMode>();
        public readonly InteractiveProperty<string> GroupNameRule = new InteractiveProperty<string>();
        public readonly InteractiveProperty<string> LabelRules = new InteractiveProperty<string>();

        public EntryRulesEditorTreeViewItem(EntityId controlId)
        {
            ControlId = controlId;
        }

        public EntityId ControlId { get; }
        public bool IsAddressablePathRuleValid { get; set; }
        public bool IsGroupNameValid { get; set; }
        public bool IsLabelRulesValid { get; set; }
    }

    public class SetOnlyEntryRulesEditorTreeViewItem
    {
        private readonly EntryRulesEditorTreeViewItem _source;

        public SetOnlyEntryRulesEditorTreeViewItem(EntryRulesEditorTreeViewItem source)
        {
            _source = source;
        }

        public EntityId ControlId => _source.ControlId;

        public bool IsAddressablePathRuleValid
        {
            get => _source.IsAddressablePathRuleValid;
            set => _source.IsAddressablePathRuleValid = value;
        }

        public bool IsGroupNameValid
        {
            get => _source.IsGroupNameValid;
            set => _source.IsGroupNameValid = value;
        }

        public bool IsLabelRulesValid
        {
            get => _source.IsLabelRulesValid;
            set => _source.IsLabelRulesValid = value;
        }

        public ISetOnlyInteractiveProperty<string> AddressablePathRule => _source.AddressablePathRule;
        public ISetOnlyInteractiveProperty<AddressingMode> AddressingMode => _source.AddressingMode;
        public ISetOnlyInteractiveProperty<string> GroupNameRule => _source.GroupNameRule;
        public ISetOnlyInteractiveProperty<string> LabelRules => _source.LabelRules;
    }
}