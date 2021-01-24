using System.Security.Cryptography.X509Certificates;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EZAddresser.Tests.Editor.Core.Domain.Models
{
    public class SettingTest : MonoBehaviour
    {
        [Test]
        public void SetPackingMode_CanSet()
        {
            const PackingMode packingMode = PackingMode.PackByAddressablesFolder;
            var setting = new Setting();
            setting.SetBasePackingMode(packingMode);
            
            Assert.That(setting.BasePackingMode.Value, Is.EqualTo(packingMode));
        }
        
        [Test]
        public void SetAddressingMode_CanSet()
        {
            const AddressingMode addressingMode = AddressingMode.AddressablePath;
            var setting = new Setting();
            setting.SetBaseAddressingMode(addressingMode);
            
            Assert.That(setting.BaseAddressingMode.Value, Is.EqualTo(addressingMode));
        }
        
        [Test]
        public void SetDefaultGroupTemplateGuid_CanSet()
        {
            var templateGuid = GUID.Generate().ToString();
            var setting = new Setting();
            setting.SetGroupTemplateGuid(templateGuid);
            
            Assert.That(setting.GroupTemplateGuid.Value, Is.EqualTo(templateGuid));
        }
    }
}
