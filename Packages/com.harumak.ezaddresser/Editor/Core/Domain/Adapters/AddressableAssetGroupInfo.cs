namespace EZAddresser.Editor.Core.Domain.Adapters
{
    public class AddressableAssetGroupInfo
    {
        public AddressableAssetGroupInfo(string guid, string name)
        {
            Guid = guid;
            Name = name;
        }

        public string Guid { get; }
        public string Name { get; }
    }
}