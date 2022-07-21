namespace SkillCraft.Core.Worlds
{
  public interface IWorldQuerier
  {
    Task<World?> GetAsync(string alias, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<World?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<World>> GetPagedAsync(Guid userId, string? search = null,
      WorldSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
