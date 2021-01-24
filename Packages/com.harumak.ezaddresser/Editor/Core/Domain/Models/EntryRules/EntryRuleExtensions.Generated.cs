using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;


namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    public static class EntryRuleExtensions
    {
        public static ReadOnlyEntryRule ToReadOnly(this EntryRule self)
        {
            return new ReadOnlyEntryRule(self);
        }
        
        public static ReadOnlyEntryRuleSet ToReadOnly(this EntryRuleSet self)
        {
            return new ReadOnlyEntryRuleSet(self);
        }

        public static ReadOnlyEntryRuleSet ToReadOnly(this IEntryRuleSet self)
        {
            return new ReadOnlyEntryRuleSet(self);
        }
        
        public static ReadOnlyEntryRuleSet ToReadOnly(this IReadOnlyEntryRuleSet self)
        {
            return new ReadOnlyEntryRuleSet((EntryRuleSet)self);
        }
    }
}