using System.Linq;
using EZAddresser.Editor.Core.Domain.Models.EntryOperationInfos;
using EZAddresser.Editor.Core.Domain.Services;
using NUnit.Framework;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class EntryOperationInfoApplyDomainServiceTest
    {
        [Test]
        public void Apply_Create_CanCreate()
        {
            // Set up services.
            var addressablesEditorService = new FakeAddressablesEditorAdapter();
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var service = new EntryOperationInfoApplyDomainService(addressablesEditorService, assetDatabaseService);

            // Set up test assets.
            const string address = "dummyAddress";
            const string createdAssetPath = "Assets/Prefabs/prefab_test.prefab";
            const string groupName = "TestPrefabs";
            var labels = new[] {"DummyLabel1", "DummyLabel2"};
            var groupTemplate = ScriptableObject.CreateInstance<AddressableAssetGroupTemplate>();
            var groupTemplateGuid =
                assetDatabaseService.CreateTestAsset("Assets/TestGroupTemplate.asset", groupTemplate);
            var assetGuid = assetDatabaseService.CreateTestAsset(createdAssetPath, new GameObject());

            // Test.
            var createOrMoveOperationInfo = new EntryCreateOrMoveOperationInfo(createdAssetPath, address,
                groupName, groupTemplateGuid, labels);
            var operationInfo = new EntryOperationInfo(createOrMoveOperationInfo, null);
            service.Apply(operationInfo);
            var group = addressablesEditorService.Groups.Values.First();
            var entry = group.Entries.First();
            Assert.That(group.Name, Is.EqualTo(groupName));
            Assert.That(entry.Guid, Is.EqualTo(assetGuid));
            Assert.That(entry.Address, Is.EqualTo(address));
            Assert.That(entry.Labels.Length, Is.EqualTo(2));
            Assert.That(entry.Labels, Contains.Item(labels[0]));
            Assert.That(entry.Labels, Contains.Item(labels[1]));
        }

        [Test]
        public void Apply_Move_CanMove()
        {
            // Set up services.
            var addressablesEditorService = new FakeAddressablesEditorAdapter();
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var service = new EntryOperationInfoApplyDomainService(addressablesEditorService, assetDatabaseService);

            // Set up test assets.
            const string beforeGroupName = "BeforeGroup";
            const string afterGroupName = "AfterGroup";
            var beforeGroupInfo = addressablesEditorService.CreateGroup(beforeGroupName, false, false, false,
                null, null);
            const string address = "dummyAddress";
            const string createdAssetPath = "Assets/Prefabs/prefab_test.prefab";
            var groupTemplate = ScriptableObject.CreateInstance<AddressableAssetGroupTemplate>();
            var groupTemplateGuid =
                assetDatabaseService.CreateTestAsset("Assets/TestGroupTemplate.asset", groupTemplate);
            var assetGuid = assetDatabaseService.CreateTestAsset(createdAssetPath, new GameObject());
            addressablesEditorService.CreateOrMoveEntry(assetGuid, beforeGroupInfo.Guid);
            var beforeGroup = addressablesEditorService.Groups.Values.First(x => x.Name.Equals(beforeGroupName));
            Assert.That(beforeGroup.Entries.Count, Is.EqualTo(1));

            // Test.
            var createOrMoveOperationInfo = new EntryCreateOrMoveOperationInfo(createdAssetPath, address,
                afterGroupName, groupTemplateGuid, null);
            var operationInfo = new EntryOperationInfo(createOrMoveOperationInfo, null);
            service.Apply(operationInfo);
            var afterGroup = addressablesEditorService.Groups.Values.First(x => x.Name.Equals(afterGroupName));
            var entry = afterGroup.Entries.First();
            Assert.That(beforeGroup.Entries.Count, Is.Zero);
            Assert.That(afterGroup.Entries.Count, Is.EqualTo(1));
            Assert.That(entry.Guid, Is.EqualTo(assetGuid));
            Assert.That(entry.Address, Is.EqualTo(address));
        }

        [Test]
        public void Apply_Remove_CanRemove()
        {
            // Set up services.
            var addressablesEditorService = new FakeAddressablesEditorAdapter();
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var service = new EntryOperationInfoApplyDomainService(addressablesEditorService, assetDatabaseService);

            // Set up test assets.
            const string groupName = "BeforeGroup";
            var beforeGroupInfo = addressablesEditorService.CreateGroup(groupName, false, false, false,
                null, null);
            const string createdAssetPath = "Assets/Prefabs/prefab_test.prefab";
            var groupTemplate = ScriptableObject.CreateInstance<AddressableAssetGroupTemplate>();
            assetDatabaseService.CreateTestAsset("Assets/TestGroupTemplate.asset", groupTemplate);
            var assetGuid = assetDatabaseService.CreateTestAsset(createdAssetPath, new GameObject());
            addressablesEditorService.CreateOrMoveEntry(assetGuid, beforeGroupInfo.Guid);
            var beforeGroup = addressablesEditorService.Groups.Values.First(x => x.Name.Equals(groupName));
            Assert.That(beforeGroup.Entries.Count, Is.EqualTo(1));

            // Test.
            var removeOperationInfo = new EntryRemoveOperationInfo(createdAssetPath);
            var operationInfo = new EntryOperationInfo(null, removeOperationInfo);
            service.Apply(operationInfo);
            Assert.That(beforeGroup.Entries.Count, Is.Zero);
            Assert.That(addressablesEditorService.Groups.Count, Is.Zero);
        }
    }
}