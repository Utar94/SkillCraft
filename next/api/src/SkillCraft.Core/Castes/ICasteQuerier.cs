namespace SkillCraft.Core.Castes
{
  public interface ICasteQuerier
  {
    Task<Caste?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Caste>> GetPagedAsync(int worldSid, string? search = null, Skill? skill = null,
      CasteSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
