namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    public partial class ReadOnlyEntryRule
    {
        public bool Validate(out string error)
        {
            return _source.Validate(out error);
        }

        public bool ValidateAddressablePathRule(out string error)
        {
            return _source.ValidateAddressablePathRule(out error);
        }

        public bool ValidateGroupNameRule(out string error)
        {
            return _source.ValidateGroupNameRule(out error);
        }
    }
}