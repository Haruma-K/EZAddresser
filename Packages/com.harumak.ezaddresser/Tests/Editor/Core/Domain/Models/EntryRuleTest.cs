using System.Linq;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using NUnit.Framework;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Models
{
    public class EntryRuleTest
    {
        [Test]
        public void SetAssetRelativePathRule_SetValidRule_CanSetAndIsValid()
        {
            const string dummyAssetPathRule = @"Dummy/(?<id>[0-9]{4})\.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(dummyAssetPathRule);
            Assert.That(rule.AddressablePathRule.Value, Is.EqualTo(dummyAssetPathRule));
            Assert.That(rule.ValidateAddressablePathRule(out _), Is.True);
        }
        
        [Test]
        public void SetAssetRelativePathRule_SetInvalidRule_CanSetAndIsInvalid()
        {
            const string dummyAssetPathRule = @"Dummy/(?<id>[0-9]{4}\.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(dummyAssetPathRule);
            Assert.That(rule.AddressablePathRule.Value, Is.EqualTo(dummyAssetPathRule));
            Assert.That(rule.ValidateAddressablePathRule(out _), Is.False);
        }
        
        [Test]
        public void SetAddressingMode_CanSet()
        {
            const AddressingMode dummyAddressingMode = AddressingMode.AddressablePath;
            var rule = new EntryRule();
            rule.SetAddressingMode(dummyAddressingMode);
            Assert.That(rule.AddressingMode.Value, Is.EqualTo(dummyAddressingMode));
        }
        
        [Test]
        public void SetGroupNameRule_SetValidRule_CanSetAndIsValid()
        {
            const string dummyGroupNameRule = "Dummy{id}";
            var rule = new EntryRule();
            rule.SetGroupNameRule(dummyGroupNameRule);
            Assert.That(rule.GroupNameRule.Value, Is.EqualTo(dummyGroupNameRule));
            Assert.That(rule.ValidateGroupNameRule(out _), Is.True);
        }
        
        [Test]
        public void SetGroupNameRule_SetInvalidRule_CanSetAndIsInvalid()
        {
            const string dummyGroupNameRule = "";
            var rule = new EntryRule();
            rule.SetGroupNameRule(dummyGroupNameRule);
            Assert.That(rule.GroupNameRule.Value, Is.EqualTo(dummyGroupNameRule));
            Assert.That(rule.ValidateGroupNameRule(out _), Is.False);
        }
        
        [Test]
        public void SetLabelRules_CanSet()
        {
            const string dummyLabelRules = "DummyLabel1,DummyLabel2";
            var rule = new EntryRule();
            rule.SetLabelRules(dummyLabelRules);
            Assert.That(rule.LabelRules.Value, Is.EqualTo(dummyLabelRules));
        }

        [Test]
        public void ValidateAssetRelativePathRule_ValidAssetPathRule_ReturnTrue()
        {
            // Set up.
            const string dummyAssetPathRule = @"Dummy/(?<id>[0-9]{4})\.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(dummyAssetPathRule);
            
            // Test.
            var result = rule.ValidateAddressablePathRule(out _);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ValidateAssetPathRule_Empty_ReturnFalse()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetAddressablePathRule(string.Empty);
            
            // Test.
            var result = rule.ValidateAddressablePathRule(out _);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void ValidateAssetPathRule_InvalidAssetPathRule_ReturnFalse()
        {
            // Set up.
            const string dummyAssetPathRule = @"Dummy/(?<id>[0-9]{4}\.prefab";
            var rule = new EntryRule();
            rule.SetAddressablePathRule(dummyAssetPathRule);
            
            // Test.
            var result = rule.ValidateAddressablePathRule(out _);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void ValidateGroupNameRule_ValidGroupNameRule_ReturnTrue()
        {
            // Set up.
            const string dummyGroupNameRule = "Dummy{id}";
            var rule = new EntryRule();
            rule.SetGroupNameRule(dummyGroupNameRule);
            
            // Test.
            var result = rule.ValidateGroupNameRule(out _);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ValidateGroupNameRule_Empty_ReturnFalse()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetGroupNameRule("");
            
            // Test.
            var result = rule.ValidateGroupNameRule(out _);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void ValidateLabelRules_Empty_ReturnTrue()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetLabelRules("");
            
            // Test.
            var result = rule.ValidateLabelRules(out _);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ValidateLabelRules_OneLabel_ReturnTrue()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetLabelRules("DummyLabel");
            
            // Test.
            var result = rule.ValidateLabelRules(out _);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ValidateLabelRules_TwoLabels_ReturnTrue()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetLabelRules("DummyLabel1,DummyLabel2");
            
            // Test.
            var result = rule.ValidateLabelRules(out _);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void ValidateLabelRules_ContainsSpace_ReturnFalse()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetLabelRules("DummyLabel1, DummyLabel2");
            
            // Test.
            var result = rule.ValidateLabelRules(out _);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void ValidateLabelRules_ContainsEmpty_ReturnFalse()
        {
            // Set up.
            var rule = new EntryRule();
            rule.SetLabelRules("DummyLabel1,");
            
            // Test.
            var result = rule.ValidateLabelRules(out _);
            Assert.That(result, Is.False);
        }
    }
}
