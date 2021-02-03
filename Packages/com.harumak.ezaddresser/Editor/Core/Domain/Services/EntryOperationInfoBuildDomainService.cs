using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryOperationInfos;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using UnityEngine.Assertions;

namespace EZAddresser.Editor.Core.Domain.Services
{
    /// <summary>
    ///     Class that builds the <see cref="EntryOperationInfo" />.
    /// </summary>
    public class EntryOperationInfoBuildDomainService
    {
        private readonly AddressablePathGenerateDomainService _addressablePathGenerateService;
        private readonly AddressGenerateDomainService _addressGenerateDomainService;
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;
        private readonly GroupNameGenerateDomainService _groupNameGenerateService;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="addressablePathGenerateService"></param>
        /// <param name="addressGenerateDomainService"></param>
        /// <param name="groupNameGenerateService"></param>
        /// <param name="assetDatabaseAdapter"></param>
        public EntryOperationInfoBuildDomainService(AddressablePathGenerateDomainService addressablePathGenerateService,
            AddressGenerateDomainService addressGenerateDomainService,
            GroupNameGenerateDomainService groupNameGenerateService, IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            Assert.IsNotNull(groupNameGenerateService);

            _addressablePathGenerateService = addressablePathGenerateService;
            _addressGenerateDomainService = addressGenerateDomainService;
            _groupNameGenerateService = groupNameGenerateService;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        /// <summary>
        ///     Build <see cref="EntryCreateOrMoveOperationInfo" />.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="defaultAddressingMode"></param>
        /// <param name="defaultPackingMode"></param>
        /// <param name="defaultGroupTemplateGuid"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public EntryCreateOrMoveOperationInfo BuildEntryCreateOrMoveInfo(string assetPath,
            AddressingMode defaultAddressingMode, PackingMode defaultPackingMode, string defaultGroupTemplateGuid,
            EntryRuleSet rules)
        {
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));
            Assert.IsFalse(string.IsNullOrEmpty(_assetDatabaseAdapter.GUIDToAssetPath(defaultGroupTemplateGuid)));

            // Do not process the folder.
            if (Directory.Exists(assetPath))
            {
                return null;
            }

            var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(assetPath);
            if (string.IsNullOrEmpty(addressablePath))
            {
                return null;
            }

            // Determine all rules.
            if (rules != null)
            {
                var info = BuildEntryCreateOrMoveInfoByRules(assetPath, rules, defaultGroupTemplateGuid);
                if (info != null)
                {
                    return info;
                }
            }

            // Apply the default settings if none of the rules are matched.
            var address =
                _addressGenerateDomainService.GenerateFromAddressablePath(addressablePath, defaultAddressingMode);
            var groupName = _groupNameGenerateService.GenerateFromAssetPath(assetPath, defaultPackingMode);
            var groupTemplateGuid = defaultGroupTemplateGuid;
            return new EntryCreateOrMoveOperationInfo(assetPath, address, groupName, groupTemplateGuid, null);
        }

        /// <summary>
        ///     Build <see cref="EntryCreateOrMoveOperationInfo" /> using <see cref="EntryRuleSet" />.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="rules"></param>
        /// <param name="defaultGroupTemplateGuid"></param>
        /// <returns></returns>
        public EntryCreateOrMoveOperationInfo BuildEntryCreateOrMoveInfoByRules(string assetPath, EntryRuleSet rules,
            string defaultGroupTemplateGuid)
        {
            Assert.IsNotNull(rules);
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));
            Assert.IsFalse(string.IsNullOrEmpty(_assetDatabaseAdapter.GUIDToAssetPath(defaultGroupTemplateGuid)));

            foreach (var rule in rules)
            {
                var info = BuildEntryCreateOrMoveInfoByRule(assetPath, rule, defaultGroupTemplateGuid);
                if (info != null)
                {
                    return info;
                }
            }

            return null;
        }

        /// <summary>
        ///     Build <see cref="EntryCreateOrMoveOperationInfo" /> using <see cref="EntryRule" />.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="rule"></param>
        /// <param name="groupTemplateGuid"></param>
        /// <returns></returns>
        public EntryCreateOrMoveOperationInfo BuildEntryCreateOrMoveInfoByRule(string assetPath, EntryRule rule,
            string groupTemplateGuid)
        {
            Assert.IsNotNull(rule);
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));
            Assert.IsFalse(string.IsNullOrEmpty(_assetDatabaseAdapter.GUIDToAssetPath(groupTemplateGuid)));

            // Do not process the folder.
            if (Directory.Exists(assetPath))
            {
                return null;
            }

            // Do not process the invalid rule.
            if (!rule.Validate(out _))
            {
                return null;
            }

            var addressablePath = _addressablePathGenerateService.GenerateFromAssetPath(assetPath);

            var regex = new Regex(rule.AddressablePathRule.Value);
            var match = regex.Match(addressablePath);
            if (!match.Success)
            {
                return null;
            }

            var address = _addressGenerateDomainService.GenerateFromAddressablePath(addressablePath,
                rule.AddressingMode.Value);
            var groupName = regex.Replace(addressablePath, rule.GroupNameRule.Value);

            // Replace '/' to '-'
            groupName = groupName.Replace("/", "-");

            var labels = new List<string>();
            if (rule.LabelRules.Value != null && rule.LabelRules.Value.Length >= 1)
            {
                var labelRules = rule.LabelRules.Value.Split(',');
                foreach (var labelRule in labelRules)
                {
                    if (string.IsNullOrEmpty(labelRule))
                    {
                        continue;
                    }

                    var label = regex.Replace(addressablePath, labelRule);
                    labels.Add(label);
                }
            }

            var createOrMoveOperationInfo =
                new EntryCreateOrMoveOperationInfo(assetPath, address, groupName, groupTemplateGuid, labels.ToArray());
            return createOrMoveOperationInfo;
        }

        /// <summary>
        ///     Build <see cref="EntryRemoveOperationInfo" />.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public EntryRemoveOperationInfo BuildEntryRemoveInfo(string assetPath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(assetPath));

            // Do not process the folder.
            if (Directory.Exists(assetPath))
            {
                return null;
            }

            return new EntryRemoveOperationInfo(assetPath);
        }
    }
}