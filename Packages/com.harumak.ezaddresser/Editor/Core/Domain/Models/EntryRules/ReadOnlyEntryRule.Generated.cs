using System.Collections.Generic;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableCollection;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    /// <summary>
    /// Read-only <see cref="EntryRule"/>.
    /// </summary>
    public partial class ReadOnlyEntryRule : Entity
    {
        private readonly EntryRule _source;

        public ReadOnlyEntryRule(EntryRule source)
        {
            Id = source.Id;
            _source = source;
        }

        /// <summary> Regular expression representing a path relative to the Addressables folder to specify the target asset. </summary>
        public IReadOnlyObservableProperty<string> AddressablePathRule => _source.AddressablePathRule;

        /// <summary> The type of method to address an asset. </summary>
        public IReadOnlyObservableProperty<AddressingMode> AddressingMode => _source.AddressingMode;

        /// <summary> The group name, which is the result of Replace() the regular expression of AddressablePathRule with this value. </summary>
        public IReadOnlyObservableProperty<string> GroupNameRule => _source.GroupNameRule;
        
        /// <summary> The label name, which is the result of Replace() the regular expression of AddressablePathRule with this value. Can be specified multiple values separated by ','.</summary>
        public IReadOnlyObservableProperty<string> LabelRules => _source.LabelRules;
    }
}
