using EZAddresser.Editor.Core.Domain.Models.Shared;

namespace EZAddresser.Editor.Core.Domain.Models.Settings
{
    public partial class Setting
    {
        public Setting()
        {
            _basePackingMode.Value = PackingMode.PackByAddressablesFolder;
            _baseAddressingMode.Value = AddressingMode.AssetNameWithoutExtensions;
        }

        public void SetBasePackingMode(PackingMode packingMode)
        {
            _basePackingMode.Value = packingMode;
        }

        public void SetBaseAddressingMode(AddressingMode addressingMode)
        {
            _baseAddressingMode.Value = addressingMode;
        }

        public void SetGroupTemplateGuid(string guid)
        {
            _groupTemplateGuid.Value = guid;
        }
    }
}