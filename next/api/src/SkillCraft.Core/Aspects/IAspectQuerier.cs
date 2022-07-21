namespace SkillCraft.Core.Aspects
{
  public interface IAspectQuerier
  {
    Task<Aspect?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Aspect>> GetPagedAsync(int worldSid, string? search = null,
      AspectSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
