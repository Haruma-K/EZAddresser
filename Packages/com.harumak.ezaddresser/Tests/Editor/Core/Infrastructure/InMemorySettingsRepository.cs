using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.Settings;
using EZAddresser.Tests.Editor.Shared;

namespace EZAddresser.Tests.Editor.Core.Infrastructure
{
    public class InMemorySettingsRepository : InMemoryRepository<Setting>, ISettingsRepository
    {
    }
}