namespace SkillCraft.Core.Customizations
{
  public interface ICustomizationQuerier
  {
    Task<Customization?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Customization>> GetPagedAsync(int worldSid, string? search = null, CustomizationType? type = null,
      CustomizationSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
