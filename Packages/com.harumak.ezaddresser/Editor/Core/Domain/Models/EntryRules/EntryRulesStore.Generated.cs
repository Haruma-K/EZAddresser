using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    /// <summary>
    /// Stores the state of the EntryRules.
    /// </summary>
    public partial class EntryRulesStore : Store
    {
        public EntryRuleSet State { get; }

        public EntryRulesStore(EntryRuleSet initialState)
        {
            State = initialState;
        }
    }
}

