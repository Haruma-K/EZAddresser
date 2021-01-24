using System.Threading.Tasks;
using EZAddresser.Editor.Foundation.LocalPersistence;
using NUnit.Framework;

namespace EZAddresser.Editor.Foundation
{
    public abstract class UnityJsonLocalPersistenceRepository<T> : IRepository<T> where T : new()
    {
        private readonly UnityJsonLocalPersistence<T> _localPersistence;

        public UnityJsonLocalPersistenceRepository(string folderPath, string fileNameWithoutExtensions)
        {
            _localPersistence = new UnityJsonLocalPersistence<T>(folderPath, fileNameWithoutExtensions)
            {
                PrettyPrint = true
            };
            if (!Exists())
            {
                SaveAsync(new T());
            }
        }

        public bool Exists()
        {
            return _localPersistence.Exists();
        }

        public T Fetch()
        {
            Assert.IsTrue(_localPersistence.Exists());

            return FetchAsync().Result;
        }

        public Task<T> FetchAsync()
        {
            Assert.IsTrue(_localPersistence.Exists());

            return _localPersistence.LoadAsync();
        }

        public void Save(T item)
        {
            SaveAsync(item).Wait();
        }

        public Task SaveAsync(T item)
        {
            return _localPersistence.SaveAsync(item);
        }
    }
}