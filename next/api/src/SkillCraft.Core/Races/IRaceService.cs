using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Payload;

namespace SkillCraft.Core.Races
{
  public interface IRaceService
  {
    Task<RaceModel> CreateAsync(CreateRacePayload payload, CancellationToken cancellationToken = default);
    Task<RaceModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RaceModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<RaceModel>> GetAsync(Guid? parentId = null, string? search = null, SizeCategory? size = null,
      RaceSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<RaceModel> UpdateAsync(Guid id, UpdateRacePayload payload, CancellationToken cancellationToken = default);
  }
}
