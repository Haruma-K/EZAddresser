using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    /// <summary>
    /// Stores the state of the Settings.
    /// </summary>
    public partial class SettingsStore : Store
    {
        public Setting State { get; }

        public SettingsStore(Setting initialState)
        {
            State = initialState;
        }
    }
}

