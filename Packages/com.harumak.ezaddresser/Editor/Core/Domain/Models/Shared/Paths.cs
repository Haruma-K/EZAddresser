namespace EZAddresser.Editor.Core.Domain.Models.Shared
{
    public static class Paths
    {
        private const string DataFolderFormat = "EZAddresser/Data";
        private const string EntryRulesRepositoryFileNameFormat = "EntryRules";
        private const string SettingsRepositoryFileNameFormat = "Settings";
        private const string DefaultGroupNameFormat = "EZAddresser";
        private const string AddressablesFolderNameFormat = "Addressables";

        public static string GetDataFolder()
        {
            return DataFolderFormat;
        }

        public static string GetEntryRulesRepositoryFileName()
        {
            return EntryRulesRepositoryFileNameFormat;
        }

        public static string GetSettingsRepositoryFileName()
        {
            return SettingsRepositoryFileNameFormat;
        }

        public static string GetDefaultGroupName()
        {
            return DefaultGroupNameFormat;
        }

        public static string GetAddressablesFolderName()
        {
            return AddressablesFolderNameFormat;
        }
    }
}