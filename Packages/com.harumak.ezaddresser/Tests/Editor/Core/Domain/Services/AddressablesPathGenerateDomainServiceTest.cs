using EZAddresser.Editor.Core.Domain.Models.Shared;
using NUnit.Framework;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class AddressablesPathGenerateDomainServiceTest
    {
        [Test]
        public void GenerateFromAssetPath_AssetPathInAddressablesFolder_Generated()
        {
            const string assetPath = "Assets/Dummy/Addressables/Test/Dummy.prefab";
            var service = new AddressablePathGenerateDomainService();
            var addressablePath = service.GenerateFromAssetPath(assetPath);

            Assert.That(addressablePath, Is.EqualTo("Test/Dummy.prefab"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AssetPathInNestedAddressablesFolder_Generated()
        {
            const string assetPath = "Assets/Dummy/Addressables/Test/Addressables/Dummy.prefab";
            var service = new AddressablePathGenerateDomainService();
            var addressablePath = service.GenerateFromAssetPath(assetPath);

            Assert.That(addressablePath, Is.EqualTo("Dummy.prefab"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AssetPathNotInAddressablesFolder_NotGenerated()
        {
            const string assetPath = "Assets/Dummy/Dummy.prefab";
            var service = new AddressablePathGenerateDomainService();
            var addressablePath = service.GenerateFromAssetPath(assetPath);

            Assert.That(addressablePath, Is.EqualTo(string.Empty));
        }
        
        [Test]
        public void GenerateFromAssetPath_NotAssetPath_NotGenerated()
        {
            const string assetPath = "NotAssets/Dummy/Addressables/Test/Dummy.prefab";
            var service = new AddressablePathGenerateDomainService();
            var addressablePath = service.GenerateFromAssetPath(assetPath);

            Assert.That(addressablePath, Is.EqualTo(string.Empty));
        }
    }
}
