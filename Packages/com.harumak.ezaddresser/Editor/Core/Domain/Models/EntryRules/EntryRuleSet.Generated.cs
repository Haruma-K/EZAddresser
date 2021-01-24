using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    public partial interface IEntryRuleSet : IObservableEntitySet<EntryRule>
    {
    }

    public partial interface IReadOnlyEntryRuleSet : IReadOnlyObservableEntitySet<EntryRule>
    {
    }
    
    /// <summary>
    /// Set of the <see cref="EntryRule"/>
    /// </summary>
    [Serializable]
    public partial class EntryRuleSet : ObservableEntitySet<EntryRule>, IEntryRuleSet, IReadOnlyEntryRuleSet
    {
        public EntryRuleSet() : base()
        {
        }

        public EntryRuleSet(IEnumerable<EntryRule> entities) : base(entities)
        {
        }
    }
}
