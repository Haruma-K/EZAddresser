using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Editor.Core.Domain.Models.Shared;
using EZAddresser.Editor.Foundation;

namespace EZAddresser.Editor.Core.Infrastructure.Repositories
{
    public class EntryRulesRepository
        : UnityJsonLocalPersistenceRepository<EntryRuleSet>, IEntryRulesRepository
    {
        public EntryRulesRepository() : base(Paths.GetDataFolder(), Paths.GetEntryRulesRepositoryFileName())
        {
        }
    }
}