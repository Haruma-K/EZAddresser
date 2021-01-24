using System.Threading.Tasks;
using EZAddresser.Editor.Foundation;

namespace EZAddresser.Tests.Editor.Shared
{
    public abstract class InMemoryRepository<TItem> : IRepository<TItem> where TItem : new()
    {
        private TItem _item;
        
        protected InMemoryRepository()
        {
            if (!Exists())
            {
                SaveAsync(new TItem());
            }
        }
        
        public bool Exists()
        {
            return _item != null;
        }

        public TItem Fetch()
        {
            return _item;
        }

        public Task<TItem> FetchAsync()
        {
            return Task.FromResult(_item);
        }

        public void Save(TItem item)
        {
            _item = item;
        }

        public Task SaveAsync(TItem item)
        {
            _item = item;
            return Task.CompletedTask;
        }
    }
}