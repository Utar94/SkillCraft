using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Languages.Payload;

namespace SkillCraft.Core.Languages
{
  public interface ILanguageService
  {
    Task<LanguageModel> CreateAsync(CreateLanguagePayload payload, CancellationToken cancellationToken = default);
    Task<LanguageModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LanguageModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<LanguageModel>> GetAsync(bool? isExotic = null, string? search = null,
      LanguageSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<LanguageModel> UpdateAsync(Guid id, UpdateLanguagePayload payload, CancellationToken cancellationToken = default);
  }
}
