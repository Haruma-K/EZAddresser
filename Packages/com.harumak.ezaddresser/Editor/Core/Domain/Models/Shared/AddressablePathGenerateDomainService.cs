using System.Text.RegularExpressions;
using UnityEngine.Assertions;

namespace EZAddresser.Editor.Core.Domain.Models.Shared
{
    /// <summary>
    ///     Class that generate addressable path(= Relative path from Addressables folder).
    /// </summary>
    public class AddressablePathGenerateDomainService
    {
        /// <summary>
        ///     Generate a addressable path from a asset path.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public string GenerateFromAssetPath(string assetPath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));

            // Match the path relative to the Addressables folder.
            // If the Addressables folder is nested, match the path relative to the deepest one.
            var regex = new Regex($"^Assets/.*{Paths.GetAddressablesFolderName()}/(?<relative_path>.+?)$",
                RegexOptions.RightToLeft);
            var matches = regex.Matches(assetPath);

            // If the asset is not contained in the Addressables folder, return null.
            if (matches.Count == 0)
            {
                return string.Empty;
            }

            return matches[matches.Count - 1].Groups["relative_path"].Value;
        }
    }
}