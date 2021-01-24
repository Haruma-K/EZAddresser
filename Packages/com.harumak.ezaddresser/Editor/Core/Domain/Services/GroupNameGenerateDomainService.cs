using System;
using System.Text.RegularExpressions;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Services
{
    /// <summary>
    ///     Class that generate group name.
    /// </summary>
    public class GroupNameGenerateDomainService
    {
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="assetDatabaseAdapter"></param>
        public GroupNameGenerateDomainService(IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        /// <summary>
        ///     Generate a group name from a asset path.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="packingMode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GenerateFromAssetPath(string assetPath, PackingMode packingMode)
        {
            switch (packingMode)
            {
                case PackingMode.PackByAddressablesFolder:
                    var addressableFolderRegex =
                        new Regex($"(?<folder_path>^Assets.*/{Paths.GetAddressablesFolderName()})/");
                    var match = addressableFolderRegex.Match(assetPath);
                    if (!match.Success)
                    {
                        return string.Empty;
                    }

                    var addressableFolderPath = match.Groups["folder_path"].Value;
                    var guid = _assetDatabaseAdapter.AssetPathToGUID(addressableFolderPath);
                    if (string.IsNullOrEmpty(guid))
                    {
                        return string.Empty;
                    }

                    guid = guid.Substring(0, 7);

                    return $"{Paths.GetDefaultGroupName()}_{guid}";
                case PackingMode.PackTogether:
                    return Paths.GetDefaultGroupName();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}