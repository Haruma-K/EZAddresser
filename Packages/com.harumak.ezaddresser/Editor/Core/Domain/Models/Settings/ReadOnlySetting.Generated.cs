using System.Collections.Generic;
using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableCollection;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    /// <summary>
    /// Read-only <see cref="Setting"/>.
    /// </summary>
    public partial class ReadOnlySetting : Entity
    {
        private readonly Setting _source;

        public ReadOnlySetting(Setting source)
        {
            Id = source.Id;
            _source = source;
        }

        /// <summary> PackingMode that is applied to assets that do not match any EntryRules. </summary>
        public IReadOnlyObservableProperty<PackingMode> BasePackingMode => _source.BasePackingMode;

        /// <summary> AddressingMode that is applied to assets that do not match any EntryRules. </summary>
        public IReadOnlyObservableProperty<AddressingMode> BaseAddressingMode => _source.BaseAddressingMode;

        /// <summary> GUID of the AddressableGroupTemplate. </summary>
        public IReadOnlyObservableProperty<string> GroupTemplateGuid => _source.GroupTemplateGuid;
    }
}
