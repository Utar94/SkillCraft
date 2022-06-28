using SkillCraft.Core.Characters;

namespace SkillCraft.Core.Repositories
{
  public interface ICharacterRepository
  {
    Task<Character?> GetAsync(Guid uuid, bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
