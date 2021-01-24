using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    /// <summary>
    /// Read-only <see cref="EntryRuleSet"/>.
    /// </summary>
    public partial class ReadOnlyEntryRuleSet : ReadOnlyObservableEntitySet<EntryRule, ReadOnlyEntryRule>
    {
        public ReadOnlyEntryRuleSet(IEntryRuleSet source) : base(source, x => x.ToReadOnly())
        {
        }
    }
}
