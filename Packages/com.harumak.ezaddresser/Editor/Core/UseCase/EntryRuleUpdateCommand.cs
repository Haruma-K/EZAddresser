using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.UseCase
{
    public readonly struct EntryRuleUpdateCommand
    {
        public string AddressablePathRule { get; }

        public AddressingMode? AddressingMode { get; }

        public string GroupNameRule { get; }

        public EntryRuleUpdateCommand(string addressablePathRule = null, AddressingMode? addressingMode = null,
            string groupNameRule = null)
        {
            AddressablePathRule = addressablePathRule;
            AddressingMode = addressingMode;
            GroupNameRule = groupNameRule;
        }
    }
}