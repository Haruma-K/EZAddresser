using EZAddresser.Editor.Core.Domain.Adapters;
using EZAddresser.Editor.Core.Domain.Models.EntryRules;
using EZAddresser.Tests.Editor.Shared;

namespace EZAddresser.Tests.Editor.Core.Infrastructure
{
    public class InMemoryEntryRulesRepository : InMemoryRepository<EntryRuleSet>, IEntryRulesRepository
    {
    }
}