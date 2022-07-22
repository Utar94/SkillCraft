namespace SkillCraft.Core.Powers
{
  public interface IPowerQuerier
  {
    Task<Power?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Power>> GetPagedAsync(int worldSid, string? search = null, IEnumerable<int>? tiers = null,
      PowerSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
