using System;
using System.IO;
using System.Text.RegularExpressions;
using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.EntryRules
{
    public partial class EntryRule
    {
        public void SetAddressablePathRule(string assetRelativePathRule)
        {
            if (assetRelativePathRule == null)
            {
                throw new ArgumentNullException(nameof(assetRelativePathRule));
            }

            _addressingMode.Value = Shared.AddressingMode.AssetNameWithoutExtensions;
            _addressablePathRule.Value = assetRelativePathRule;
        }

        public void SetAddressingMode(AddressingMode addressingMode)
        {
            _addressingMode.Value = addressingMode;
        }

        public void SetGroupNameRule(string groupNameRule)
        {
            if (groupNameRule == null)
            {
                throw new ArgumentNullException(nameof(groupNameRule));
            }

            _groupNameRule.Value = groupNameRule;
        }

        public void SetLabelRules(string labelRules)
        {
            if (labelRules == null)
            {
                throw new ArgumentNullException(nameof(labelRules));
            }

            _labelRules.Value = labelRules;
        }

        public bool Validate(out string error)
        {
            if (!ValidateAddressablePathRule(out error))
            {
                return false;
            }

            if (!ValidateGroupNameRule(out error))
            {
                return false;
            }

            if (!ValidateLabelRules(out error))
            {
                return false;
            }

            return true;
        }

        public bool ValidateAddressablePathRule(out string error)
        {
            if (string.IsNullOrEmpty(_addressablePathRule.Value))
            {
                error = $"{nameof(AddressablePathRule)} is null or empty.";
                return false;
            }

            try
            {
                var _ = new Regex(_addressablePathRule.Value);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidateGroupNameRule(out string error)
        {
            if (string.IsNullOrEmpty(_groupNameRule.Value))
            {
                error = $"{nameof(GroupNameRule)} is null or empty.";
                return false;
            }

            if (_groupNameRule.Value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                error = $"{nameof(GroupNameRule)} contains characters that cannot be used as file names.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidateLabelRules(out string error)
        {
            if (string.IsNullOrEmpty(_labelRules.Value))
            {
                error = null;
                return true;
            }
            var labelRules = _labelRules.Value.Split(',');
            for (var i = 0; i < labelRules.Length; i++)
            {
                var labelRule = labelRules[i];
                if (string.IsNullOrEmpty(labelRule))
                {
                    error = $"{nameof(LabelRules)}[{i}] is null or empty.";
                    return false;
                }

                if (labelRule.Contains(" "))
                {
                    error = $"{nameof(LabelRules)}[{i}] contains space.";
                    return false;
                }
            }

            error = null;
            return true;
        }

        public bool ValidateLabelRules()
        {
            return true;
        }
    }
}