using System.IO;
using EZAddresser.Editor.Foundation.Serialization;

namespace EZAddresser.Editor.Foundation.LocalPersistence
{
    internal class UnityJsonLocalPersistence<TEntityCollection> : TextSerializePersistenceBase<TEntityCollection>
    {
        private const string Extension = ".json";

        private readonly UnityJsonSerializer<TEntityCollection> _serializer =
            new UnityJsonSerializer<TEntityCollection>();

        public UnityJsonLocalPersistence(string folderPath, string fileNameWithoutExtensions)
            : base($"{Path.Combine(folderPath, fileNameWithoutExtensions)}{Extension}")
        {
        }

        public bool PrettyPrint
        {
            get => _serializer.PrettyPrint;
            set => _serializer.PrettyPrint = value;
        }

        protected override ISerializer<TEntityCollection, string> Serializer => _serializer;
    }
}