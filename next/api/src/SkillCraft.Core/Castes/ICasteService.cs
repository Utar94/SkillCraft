using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Castes.Payload;

namespace SkillCraft.Core.Castes
{
  public interface ICasteService
  {
    Task<CasteModel> CreateAsync(CreateCastePayload payload, CancellationToken cancellationToken = default);
    Task<CasteModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CasteModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<CasteModel>> GetAsync(string? search = null, Skill? skill = null,
      CasteSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<CasteModel> UpdateAsync(Guid id, UpdateCastePayload payload, CancellationToken cancellationToken = default);
  }
}
