using Microsoft.EntityFrameworkCore;
using SkillCraft.Core;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Repositories;
using SkillCraft.Infrastructure.Extensions;

namespace SkillCraft.Infrastructure.Repositories
{
  internal class CharacterRepository : ICharacterRepository
  {
    public CharacterRepository(IDbContext dbContext)
    {
      DbContext = dbContext;
    }

    protected IDbContext DbContext { get; }

    public async Task<Character?> GetAsync(Guid uuid, bool readOnly = false, CancellationToken cancellationToken = default)
    {
      return await DbContext.Characters
        .ApplyTracking(readOnly)
        .AsSplitQuery()
        .Include(x => x.Aspect1)
        .Include(x => x.Aspect2)
        .Include(x => x.Caste)
        .Include(x => x.Education)
        .Include(x => x.Nature)
        .Include(x => x.Race)
        .Include(x => x.Conditions).ThenInclude(x => x.Condition)
        .Include(x => x.Customizations)
        .Include(x => x.Languages)
        .Include(x => x.Powers).ThenInclude(x => x.Power)
        .Include(x => x.Talents).ThenInclude(x => x.Talent).ThenInclude(x => x!.Class)
        .Include(x => x.Talents).ThenInclude(x => x.Talent).ThenInclude(x => x!.Options)
        .Include(x => x.Talents).ThenInclude(x => x.Option)
        .SingleOrDefaultAsync(x => x.Uuid == uuid, cancellationToken);
    }
  }
}
