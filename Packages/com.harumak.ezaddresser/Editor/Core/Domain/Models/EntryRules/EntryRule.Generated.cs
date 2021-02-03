using System;
using System.Collections.Generic;
using UnityEngine;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Foundation.Observable.ObservableCollection;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    /// <summary>
    /// Rules for creating an entry.
    /// </summary>
    [Serializable]
    public partial class EntryRule : Entity
    {
        [SerializeField] private ObservableProperty<string> _addressablePathRule = new ObservableProperty<string>();
        [SerializeField] private ObservableProperty<AddressingMode> _addressingMode = new ObservableProperty<AddressingMode>();
        [SerializeField] private ObservableProperty<string> _groupNameRule = new ObservableProperty<string>();
        [SerializeField] private ObservableProperty<string> _labelRules = new ObservableProperty<string>();

        /// <summary> Regular expression representing a path relative to the Addressables folder to specify the target asset. </summary>
        public IReadOnlyObservableProperty<string> AddressablePathRule => _addressablePathRule;

        /// <summary> The type of method to address an asset. </summary>
        public IReadOnlyObservableProperty<AddressingMode> AddressingMode => _addressingMode;

        /// <summary> The group name, which is the result of Replace() the regular expression of AddressablePathRule with this value. </summary>
        public IReadOnlyObservableProperty<string> GroupNameRule => _groupNameRule;

        /// <summary> The label name, which is the result of Replace() the regular expression of AddressablePathRule with this value. Can be specified multiple values separated by ','.</summary>
        public IReadOnlyObservableProperty<string> LabelRules => _labelRules;
    }
}
