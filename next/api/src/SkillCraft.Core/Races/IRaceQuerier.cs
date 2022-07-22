namespace SkillCraft.Core.Races
{
  public interface IRaceQuerier
  {
    Task<Race?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Race>> GetPagedAsync(int worldSid, Guid? parentId = null, string? search = null, SizeCategory? size = null,
      RaceSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
