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
    }
}