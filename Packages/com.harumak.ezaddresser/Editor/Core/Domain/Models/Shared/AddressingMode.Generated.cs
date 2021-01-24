using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Shared
{
    /// <summary>
    /// The type of method to address an asset.
    /// </summary>
    public enum AddressingMode
    {
        AssetName = 0,
        AssetNameWithoutExtensions = 1,
        AddressablePath = 2,
        AddressablePathWithoutExtensions = 3
    }
}

