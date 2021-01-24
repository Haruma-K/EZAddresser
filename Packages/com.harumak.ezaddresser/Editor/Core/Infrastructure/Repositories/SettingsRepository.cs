using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Foundation;

namespace EZAddresser.Editor.Core.Infrastructure.Repositories
{
    public class SettingsRepository
        : UnityJsonLocalPersistenceRepository<Setting>, ISettingsRepository
    {
        public SettingsRepository() : base(Paths.GetDataFolder(), Paths.GetSettingsRepositoryFileName())
        {
        }
    }
}