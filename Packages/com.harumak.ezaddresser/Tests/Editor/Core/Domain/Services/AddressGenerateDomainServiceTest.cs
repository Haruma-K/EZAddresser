using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.Domain.Services;
using NUnit.Framework;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class AddressGenerateDomainServiceTest
    {
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsFileName_Generated()
        {
            const string addressablePath = "Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AssetName);
            
            Assert.That(address, Is.EqualTo("Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsFileNameWithoutExtensions_Generated()
        {
            const string addressablePath = "Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AssetNameWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Test"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsRelativePath_Generated()
        {
            const string addressablePath = "Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AddressablePath);
            
            Assert.That(address, Is.EqualTo("Dummy/Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsRelativePathWithoutExtensions_Generated()
        {
            const string addressablePath = "Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AddressablePathWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Dummy/Test"));
        }

        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsFileName_FileNameOnly_Generated()
        {
            const string addressablePath = "Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AssetName);
            
            Assert.That(address, Is.EqualTo("Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsFileNameWithoutExtensions_FileNameOnly_Generated()
        {
            const string addressablePath = "Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AssetNameWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Test"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsRelativePath_FileNameOnly_Generated()
        {
            const string addressablePath = "Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AddressablePath);
            
            Assert.That(address, Is.EqualTo("Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAddressblePath_AddressingModeIsRelativePathWithoutExtensions_FileNameOnly_Generated()
        {
            const string addressablePath = "Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAddressablePath(addressablePath, AddressingMode.AddressablePathWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Test"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AddressingModeIsFileName_Generated()
        {
            var assetPath = $"Assets/Dummy/{Paths.GetAddressablesFolderName()}/Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAssetPath(assetPath, AddressingMode.AssetName);
            
            Assert.That(address, Is.EqualTo("Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AddressingModeIsFileNameWithoutExtensions_Generated()
        {
            var assetPath = $"Assets/Dummy/{Paths.GetAddressablesFolderName()}/Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAssetPath(assetPath, AddressingMode.AssetNameWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Test"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AddressingModeIsRelativePath_Generated()
        {
            var assetPath = $"Assets/Dummy/{Paths.GetAddressablesFolderName()}/Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAssetPath(assetPath, AddressingMode.AddressablePath);
            
            Assert.That(address, Is.EqualTo("Dummy/Test.prefab"));
        }
        
        [Test]
        public void GenerateFromAssetPath_AddressingModeIsRelativePathWithoutExtensions_Generated()
        {
            var assetPath = $"Assets/Dummy/{Paths.GetAddressablesFolderName()}/Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAssetPath(assetPath, AddressingMode.AddressablePathWithoutExtensions);
            
            Assert.That(address, Is.EqualTo("Dummy/Test"));
        }
        
        [Test]
        public void GenerateFromAssetPath_NotAssetsFolder_NotGenerated()
        {
            var assetPath = $"NotAssets/Dummy/{Paths.GetAddressablesFolderName()}/Dummy/Test.prefab";
            var service = CreateAddressGenerateService();
            var address = service.GenerateFromAssetPath(assetPath, AddressingMode.AssetName);
            
            Assert.That(address, Is.EqualTo(string.Empty));
        }
        
        private AddressGenerateDomainService CreateAddressGenerateService()
        {
            var addressablesPathGenerateService = new AddressablePathGenerateDomainService();
            var addressGenerateService = new AddressGenerateDomainService(addressablesPathGenerateService);
            return addressGenerateService;
        }
    }
}
