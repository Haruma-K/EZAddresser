namespace EZAddresser.Editor.Core.Domain.Adapters
{
    public class AddressableAssetEntryInfo
    {
        public AddressableAssetEntryInfo(string guid, string address)
        {
            Guid = guid;
            Address = address;
        }

        public string Guid { get; }
        public string Address { get; }
    }
}