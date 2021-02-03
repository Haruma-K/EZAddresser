using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using UnityEngine.Assertions;

namespace EZAddresser.Editor.Core.UseCase
{
    public class EntryRulesService
    {
        private readonly IEntryRulesRepository _entryRulesRepository;
        private readonly EntryRulesStore _entryRulesStore;

        public EntryRulesService(IEntryRulesRepository entryRulesRepository)
        {
            _entryRulesRepository = entryRulesRepository;

            // Initialize the store.
            var initialState = _entryRulesRepository.Fetch();
            _entryRulesStore = new EntryRulesStore(initialState);
        }

        public IReadOnlyObservableProperty<bool> HasUnsavedChanges => _entryRulesStore.HasUnsavedChanges;

        public ReadOnlyEntryRuleSet GetState()
        {
            return _entryRulesStore.State.ToReadOnly();
        }

        public ReadOnlyEntryRule AddRule(EntryRuleUpdateCommand? command = null)
        {
            var entryRule = new EntryRule();
            if (command.HasValue)
            {
                UpdateRuleInternal(entryRule, command.Value);
            }

            _entryRulesStore.State.Add(entryRule);
            _entryRulesStore.MarkAsDirty();
            return entryRule.ToReadOnly();
        }

        public void UpdateRule(EntityId entryRuleId, EntryRuleUpdateCommand command)
        {
            Assert.IsTrue(_entryRulesStore.State.Contains(entryRuleId));

            var entryRule = _entryRulesStore.State.Get(entryRuleId);
            UpdateRuleInternal(entryRule, command);

            _entryRulesStore.MarkAsDirty();
        }

        public void RemoveRule(EntityId entryRuleId)
        {
            Assert.IsTrue(_entryRulesStore.State.Contains(entryRuleId));
            _entryRulesStore.State.Remove(entryRuleId);
            _entryRulesStore.MarkAsDirty();
        }

        public void Save()
        {
            if (!_entryRulesStore.HasUnsavedChanges.Value)
            {
                return;
            }

            _entryRulesRepository.Save(_entryRulesStore.State);
            _entryRulesStore.MarkAsSaved();
        }

        private void UpdateRuleInternal(EntryRule entryRule, EntryRuleUpdateCommand command)
        {
            if (command.AddressablePathRule != null)
            {
                entryRule.SetAddressablePathRule(command.AddressablePathRule);
            }

            if (command.AddressingMode.HasValue)
            {
                entryRule.SetAddressingMode(command.AddressingMode.Value);
            }

            if (command.GroupNameRule != null)
            {
                entryRule.SetGroupNameRule(command.GroupNameRule);
            }

            if (command.LabelRules != null)
            {
                entryRule.SetLabelRules(command.LabelRules);
            }
        }
    }
}