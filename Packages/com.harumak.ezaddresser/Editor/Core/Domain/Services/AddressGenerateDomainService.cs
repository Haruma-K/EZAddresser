using System;
using System.IO;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Services
{
    /// <summary>
    ///     Class that generate address.
    /// </summary>
    public class AddressGenerateDomainService
    {
        private readonly AddressablePathGenerateDomainService _addressablePathGenerateService;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="addressablePathGenerateService"></param>
        public AddressGenerateDomainService(AddressablePathGenerateDomainService addressablePathGenerateService)
        {
            _addressablePathGenerateService = addressablePathGenerateService;
        }

        /// <summary>
        ///     Generate a address from a asset path.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="addressingMode"></param>
        /// <returns></returns>
        public string GenerateFromAssetPath(string assetPath, AddressingMode addressingMode)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return string.Empty;
            }

            var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(assetPath);
            if (string.IsNullOrEmpty(addressablePath))
            {
                return string.Empty;
            }

            return GenerateFromAddressablePath(addressablePath, addressingMode);
        }

        /// <summary>
        ///     Generate a address from a addressables path.
        /// </summary>
        /// <param name="addressablePath"></param>
        /// <param name="addressingMode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GenerateFromAddressablePath(string addressablePath, AddressingMode addressingMode)
        {
            if (string.IsNullOrEmpty(addressablePath))
            {
                return string.Empty;
            }

            switch (addressingMode)
            {
                case AddressingMode.AssetName:
                    return Path.GetFileName(addressablePath);
                case AddressingMode.AssetNameWithoutExtensions:
                    return Path.GetFileNameWithoutExtension(addressablePath);
                case AddressingMode.AddressablePath:
                    return addressablePath;
                case AddressingMode.AddressablePathWithoutExtensions:
                    return Path.ChangeExtension(addressablePath, null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(addressingMode), addressingMode, null);
            }
        }
    }
}