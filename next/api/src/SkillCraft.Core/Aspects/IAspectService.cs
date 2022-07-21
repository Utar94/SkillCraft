using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Aspects.Payload;

namespace SkillCraft.Core.Aspects
{
  public interface IAspectService
  {
    Task<AspectModel> CreateAsync(CreateAspectPayload payload, CancellationToken cancellationToken = default);
    Task<AspectModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AspectModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<AspectModel>> GetAsync(string? search = null,
      AspectSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<AspectModel> UpdateAsync(Guid id, UpdateAspectPayload payload, CancellationToken cancellationToken = default);
  }
}
