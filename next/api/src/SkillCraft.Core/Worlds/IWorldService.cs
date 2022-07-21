using SkillCraft.Core.Worlds.Models;
using SkillCraft.Core.Worlds.Payload;

namespace SkillCraft.Core.Worlds
{
  public interface IWorldService
  {
    Task<WorldModel> CreateAsync(CreateWorldPayload payload, CancellationToken cancellationToken = default);
    Task<WorldModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<WorldModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<WorldModel>> GetAsync(string? search = null,
      WorldSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<WorldModel> UpdateAsync(Guid id, UpdateWorldPayload payload, CancellationToken cancellationToken = default);
  }
}
