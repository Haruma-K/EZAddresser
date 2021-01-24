using System;
using System.Collections.Generic;
using UnityEngine;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Foundation.Observable.ObservableCollection;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    /// <summary>
    /// Settings.
    /// </summary>
    [Serializable]
    public partial class Setting : Entity
    {
        [SerializeField] private ObservableProperty<PackingMode> _basePackingMode = new ObservableProperty<PackingMode>();
        [SerializeField] private ObservableProperty<AddressingMode> _baseAddressingMode = new ObservableProperty<AddressingMode>();
        [SerializeField] private ObservableProperty<string> _groupTemplateGuid = new ObservableProperty<string>();

        /// <summary> PackingMode that is applied to assets that do not match any EntryRules. </summary>
        public IReadOnlyObservableProperty<PackingMode> BasePackingMode => _basePackingMode;

        /// <summary> AddressingMode that is applied to assets that do not match any EntryRules. </summary>
        public IReadOnlyObservableProperty<AddressingMode> BaseAddressingMode => _baseAddressingMode;

        /// <summary> GUID of the AddressableGroupTemplate. </summary>
        public IReadOnlyObservableProperty<string> GroupTemplateGuid => _groupTemplateGuid;
    }
}
