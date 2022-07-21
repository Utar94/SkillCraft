using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Educations.Payload;

namespace SkillCraft.Core.Educations
{
  public interface IEducationService
  {
    Task<EducationModel> CreateAsync(CreateEducationPayload payload, CancellationToken cancellationToken = default);
    Task<EducationModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EducationModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<EducationModel>> GetAsync(string? search = null, Skill? skill = null,
      EducationSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<EducationModel> UpdateAsync(Guid id, UpdateEducationPayload payload, CancellationToken cancellationToken = default);
  }
}
