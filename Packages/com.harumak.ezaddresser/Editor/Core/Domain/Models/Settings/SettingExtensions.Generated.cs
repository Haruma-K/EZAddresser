using EZAddresser.Editor.Foundation.DomainModel;
using EZAddresser.Editor.Core.Domain.Models.Shared;


namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    public static class SettingExtensions
    {
        public static ReadOnlySetting ToReadOnly(this Setting self)
        {
            return new ReadOnlySetting(self);
        }
        
        public static ReadOnlySettingSet ToReadOnly(this SettingSet self)
        {
            return new ReadOnlySettingSet(self);
        }

        public static ReadOnlySettingSet ToReadOnly(this ISettingSet self)
        {
            return new ReadOnlySettingSet(self);
        }
        
        public static ReadOnlySettingSet ToReadOnly(this IReadOnlySettingSet self)
        {
            return new ReadOnlySettingSet((SettingSet)self);
        }
    }
}