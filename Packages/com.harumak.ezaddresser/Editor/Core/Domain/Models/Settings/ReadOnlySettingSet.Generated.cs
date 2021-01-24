using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    /// <summary>
    /// Read-only <see cref="SettingSet"/>.
    /// </summary>
    public partial class ReadOnlySettingSet : ReadOnlyObservableEntitySet<Setting, ReadOnlySetting>
    {
        public ReadOnlySettingSet(ISettingSet source) : base(source, x => x.ToReadOnly())
        {
        }
    }
}
