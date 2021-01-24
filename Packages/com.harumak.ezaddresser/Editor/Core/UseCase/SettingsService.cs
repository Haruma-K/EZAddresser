using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Foundation.Observable.ObservableProperty;

namespace EZAddresser.Editor.Core.UseCase
{
    public class SettingsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly SettingsStore _settingsStore;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;

            // Initialize the store.
            var initialState = _settingsRepository.Fetch();
            _settingsStore = new SettingsStore(initialState);
        }

        public IReadOnlyObservableProperty<bool> HasUnsavedChanges => _settingsStore.HasUnsavedChanges;

        public ReadOnlySetting GetState()
        {
            return _settingsStore.State.ToReadOnly();
        }

        public void UpdateSetting(PackingMode? packingMode = null, AddressingMode? addressingMode = null,
            string defaultGroupTemplateGuid = null)
        {
            var state = _settingsStore.State;

            if (packingMode.HasValue)
            {
                state.SetBasePackingMode(packingMode.Value);
            }

            if (addressingMode.HasValue)
            {
                state.SetBaseAddressingMode(addressingMode.Value);
            }

            if (defaultGroupTemplateGuid != null)
            {
                state.SetGroupTemplateGuid(defaultGroupTemplateGuid);
            }

            _settingsStore.MarkAsDirty();
        }

        public void Save()
        {
            if (!_settingsStore.HasUnsavedChanges.Value)
            {
                return;
            }

            _settingsRepository.Save(_settingsStore.State);
            _settingsStore.MarkAsSaved();
        }
    }
}