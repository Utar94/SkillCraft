using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Customizations.Payload;

namespace SkillCraft.Core.Customizations
{
  public interface ICustomizationService
  {
    Task<CustomizationModel> CreateAsync(CreateCustomizationPayload payload, CancellationToken cancellationToken = default);
    Task<CustomizationModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomizationModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<CustomizationModel>> GetAsync(string? search = null, CustomizationType? type = null,
      CustomizationSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<CustomizationModel> UpdateAsync(Guid id, UpdateCustomizationPayload payload, CancellationToken cancellationToken = default);
  }
}
