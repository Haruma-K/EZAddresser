using EZAddresser.Editor.Foundation.Observable.ObservableProperty;

namespace EZAddresser.Editor.Foundation.DomainModel
{
    public abstract class Store
    {
        private readonly ObservableProperty<bool> _hasUnsavedChanges = new ObservableProperty<bool>();
        public IReadOnlyObservableProperty<bool> HasUnsavedChanges => _hasUnsavedChanges;

        public void MarkAsDirty()
        {
            _hasUnsavedChanges.Value = true;
        }

        public void MarkAsSaved()
        {
            _hasUnsavedChanges.Value = false;
        }
    }
}