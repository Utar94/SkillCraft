namespace SkillCraft.Core.Natures
{
  public interface INatureQuerier
  {
    Task<Nature?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Nature>> GetPagedAsync(int worldSid, Attribute? attribute = null, string? search = null,
      NatureSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
