using SkillCraft.Core.Talents.Models;
using SkillCraft.Core.Talents.Payload;

namespace SkillCraft.Core.Talents
{
  public interface ITalentService
  {
    Task<TalentModel> CreateAsync(CreateTalentPayload payload, CancellationToken cancellationToken = default);
    Task<TalentModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TalentModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<TalentModel>> GetAsync(bool? multipleAcquisition = null, string? search = null, Skill? skill = null, IEnumerable<int>? tiers = null,
      TalentSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<TalentModel> UpdateAsync(Guid id, UpdateTalentPayload payload, CancellationToken cancellationToken = default);
  }
}
