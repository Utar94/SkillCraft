using SkillCraft.Core.Powers.Models;
using SkillCraft.Core.Powers.Payload;

namespace SkillCraft.Core.Powers
{
  public interface IPowerService
  {
    Task<PowerModel> CreateAsync(CreatePowerPayload payload, CancellationToken cancellationToken = default);
    Task<PowerModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PowerModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<PowerModel>> GetAsync(string? search = null, IEnumerable<int>? tiers = null,
      PowerSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<PowerModel> UpdateAsync(Guid id, UpdatePowerPayload payload, CancellationToken cancellationToken = default);
  }
}
