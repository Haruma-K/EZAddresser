using System.Runtime.CompilerServices;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Core.Domain.Services;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Services
{
    public class EntryOperationInfoBuildDomainServiceTest
    {
        private EntryOperationInfoBuildDomainService CreateBuildService(
            FakeAssetDatabaseAdapter assetDatabaseAdapter = null)
        {
            var addressablesPathGenerateService = new AddressablePathGenerateDomainService();
            var addressGenerateService = new AddressGenerateDomainService(addressablesPathGenerateService);
            assetDatabaseAdapter = assetDatabaseAdapter ?? new FakeAssetDatabaseAdapter();
            var groupNameService = new GroupNameGenerateDomainService(assetDatabaseAdapter);
            return new EntryOperationInfoBuildDomainService(addressablesPathGenerateService, addressGenerateService,
                groupNameService, assetDatabaseAdapter);
        }

        #region BuildEntryRemoveInfo

        [Test]
        public void BuildEntryRemoveInfo_RuleIsNotMatched_ReturnNull()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseService = new FakeAssetDatabaseAdapter();
            var service = CreateBuildService(assetDatabaseService);

            // Test.
            var entryOperationInfo = service.BuildEntryRemoveInfo(dummyAssetPath);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
        }

        #endregion

        #region BuildEntryCreateOrMoveInfo

        [Test]
        public void BuildEntryCreateOrMoveInfo_AddressingModeIsFileName_SetCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string address = "prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackTogether;

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, null);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(address));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_AddressingModeIsAddressablePath_SetCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string address = "Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AddressablePath;
            var packingMode = PackingMode.PackTogether;

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, null);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(address));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_PackingModeIsPackTogether_SetCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackTogether;

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, null);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.GroupName, Is.EqualTo(Paths.GetDefaultGroupName()));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_PackingModeIsPackByAddressablesFolder_SetCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackByAddressablesFolder;

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, null);
            var groupName = $"{Paths.GetDefaultGroupName()}_{addressableFolderGuid.Substring(0, 7)}";
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.GroupName, Is.EqualTo(groupName));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_SetGroupTemplateGuid_SetCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackByAddressablesFolder;

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, null);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.GroupTemplateGuid, Is.EqualTo(addressableFolderGuid));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_ExitsMatchedRule_UseRule()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string address = "Prefabs/prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackTogether;
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            var rules = new EntryRuleSet();
            rules.Add(rule);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, rules);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(address));
            Assert.That(entryOperationInfo.GroupName, Is.EqualTo("Group_0001"));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfo_NoRulesMatched_UseDefault()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string address = "prefab_dummy_0001.prefab";
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);
            var addressingMode = AddressingMode.AssetName;
            var packingMode = PackingMode.PackTogether;
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"__Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AssetName);
            rule.SetGroupNameRule("Group_${id}");
            var rules = new EntryRuleSet();
            rules.Add(rule);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfo(dummyAssetPath, addressingMode, packingMode,
                addressableFolderGuid, rules);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(address));
        }

        #endregion

        #region BuildEntryCreateOrMoveInfoByRules

        [Test]
        public void BuildEntryCreateOrMoveInfoByRules_NoMatchedRules_ReturnNull()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"__Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AssetName);
            rule.SetGroupNameRule("Group_${id}");
            var rules = new EntryRuleSet();
            rules.Add(rule);
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRules(dummyAssetPath, rules,
                addressableFolderGuid);
            Assert.That(entryOperationInfo, Is.Null);
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRules_ExistsMatchedRule_BuildInfoCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string dummyAssetFileName = "prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AssetName);
            rule.SetGroupNameRule("Group_${id}");
            var rules = new EntryRuleSet();
            rules.Add(rule);
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRules(dummyAssetPath, rules,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(dummyAssetFileName));
        }

        #endregion

        #region BuildEntryCreateOrMoveInfoByRule

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_AddressingModeIsFileName_AddressedByFileName()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string dummyAssetFileName = "prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AssetName);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(dummyAssetFileName));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_AddressingModeIsAddressablePath_AddressedByAddressablePath()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string addressablePath = "Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Address, Is.EqualTo(addressablePath));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_SetGroupName_ReflectedCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            const string groupName = "Group_0001";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.GroupName, Is.EqualTo(groupName));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_SetNoLabels_ReflectedCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Labels, Is.Empty);
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_SetLabel_ReflectedCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            rule.SetLabelRules("DummyLabel1");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Labels.Length, Is.EqualTo(1));
            Assert.That(entryOperationInfo.Labels, Contains.Item("DummyLabel1"));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_SetTwoLabels_ReflectedCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            rule.SetLabelRules("DummyLabel1,DummyLabel2");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.Labels.Length, Is.EqualTo(2));
            Assert.That(entryOperationInfo.Labels, Contains.Item("DummyLabel1"));
            Assert.That(entryOperationInfo.Labels, Contains.Item("DummyLabel2"));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_InvalidLabelRuls_ReturnNull()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            rule.SetLabelRules("DummyLabel1,");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo, Is.Null);
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_SetGroupTemplateGuid_ReflectedCorrectly()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"Prefabs/prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AddressablePath);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo.AssetPath, Is.EqualTo(dummyAssetPath));
            Assert.That(entryOperationInfo.GroupTemplateGuid, Is.EqualTo(addressableFolderGuid));
        }

        [Test]
        public void BuildEntryCreateOrMoveInfoByRule_RuleIsNotMatched_ReturnNull()
        {
            // Set up.
            var dummyFolderPath = $"Assets/{Paths.GetAddressablesFolderName()}";
            var dummyAssetPath = $"{dummyFolderPath}/Prefabs/prefab_dummy_0001.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(@"___prefab_dummy_(?<id>[0-9]{4})\.prefab");
            rule.SetAddressingMode(AddressingMode.AssetName);
            rule.SetGroupNameRule("Group_${id}");
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var addressableFolderGuid = assetDatabaseAdapter.CreateTestAsset(dummyFolderPath, new Object());
            var service = CreateBuildService(assetDatabaseAdapter);

            // Test.
            var entryOperationInfo = service.BuildEntryCreateOrMoveInfoByRule(dummyAssetPath, rule,
                addressableFolderGuid);
            Assert.That(entryOperationInfo, Is.Null);
        }

        #endregion
    }
}