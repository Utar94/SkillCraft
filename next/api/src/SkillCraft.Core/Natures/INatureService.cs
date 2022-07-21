using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Natures.Payload;

namespace SkillCraft.Core.Natures
{
  public interface INatureService
  {
    Task<NatureModel> CreateAsync(CreateNaturePayload payload, CancellationToken cancellationToken = default);
    Task<NatureModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NatureModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<NatureModel>> GetAsync(Attribute? attribute = null, string? search = null,
      NatureSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<NatureModel> UpdateAsync(Guid id, UpdateNaturePayload payload, CancellationToken cancellationToken = default);
  }
}
