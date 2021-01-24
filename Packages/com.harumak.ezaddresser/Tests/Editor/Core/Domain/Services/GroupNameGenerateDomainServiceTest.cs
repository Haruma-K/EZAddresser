using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.Domain.Services;
using NUnit.Framework;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class GroupNameGenerateDomainServiceTest
    {
        [Test]
        public void GenerateFromAssetPath_PackTogether_Generated()
        {
            var folderPath = "Assets/Dummy/Addressables";
            var assetPath = $"{folderPath}/Dummy/Test.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var groupNameGenerateService = CreateGroupNameGenerateDomainService(assetDatabaseService);
            var groupName = groupNameGenerateService.GenerateFromAssetPath(assetPath, PackingMode.PackTogether);

            Assert.That(groupName, Is.EqualTo(Paths.GetDefaultGroupName()));
        }
        
        [Test]
        public void GenerateFromAssetPath_PackByAddressablesFolder_Generated()
        {
            var folderPath = "Assets/Dummy/Addressables";
            var assetPath = $"{folderPath}/Dummy/Test.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var folderGuid = assetDatabaseService.CreateTestAsset(folderPath, new Object());
            var groupNameGenerateService = CreateGroupNameGenerateDomainService(assetDatabaseService);
            var groupName = groupNameGenerateService.GenerateFromAssetPath(assetPath, PackingMode.PackByAddressablesFolder);

            Assert.That(groupName, Is.EqualTo($"{Paths.GetDefaultGroupName()}_{folderGuid.Substring(0, 7)}"));
        }
        
        [Test]
        public void GenerateFromAssetPath_PackByAddressablesFolder_NestedFolder_Generated()
        {
            var folderPath = "Assets/Dummy/Addressables/Dummy/Addressables";
            var assetPath = $"{folderPath}/Dummy/Test.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var folderGuid = assetDatabaseService.CreateTestAsset(folderPath, new Object());
            var groupNameGenerateService = CreateGroupNameGenerateDomainService(assetDatabaseService);
            var groupName = groupNameGenerateService.GenerateFromAssetPath(assetPath, PackingMode.PackByAddressablesFolder);

            Assert.That(groupName, Is.EqualTo($"{Paths.GetDefaultGroupName()}_{folderGuid.Substring(0, 7)}"));
        }
        
        [Test]
        public void GenerateFromAssetPath_PackByAddressablesFolder_NotAssetsFolder_ReturnEmpty()
        {
            var folderPath = "NotAssets/Dummy/Addressables";
            var assetPath = $"{folderPath}/Dummy/Test.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            assetDatabaseService.CreateTestAsset(folderPath, new Object());
            var groupNameGenerateService = CreateGroupNameGenerateDomainService(assetDatabaseService);
            var groupName = groupNameGenerateService.GenerateFromAssetPath(assetPath, PackingMode.PackByAddressablesFolder);

            Assert.That(groupName, Is.EqualTo(string.Empty));
        }
        
        [Test]
        public void GenerateFromAssetPath_PackByAddressablesFolder_FolderNotExists_ReturnEmpty()
        {
            var folderPath = "Assets/Dummy/Addressables";
            var assetPath = $"{folderPath}/Dummy/Test.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var groupNameGenerateService = CreateGroupNameGenerateDomainService(assetDatabaseService);
            var groupName = groupNameGenerateService.GenerateFromAssetPath(assetPath, PackingMode.PackByAddressablesFolder);

            Assert.That(groupName, Is.EqualTo(string.Empty));
        }

        private GroupNameGenerateDomainService CreateGroupNameGenerateDomainService(
            FakeAssetDatabaseAdapter assetDatabaseAdapter)
        {
            return new GroupNameGenerateDomainService(assetDatabaseAdapter);
        }
    }
}
