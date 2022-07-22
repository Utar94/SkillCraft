namespace SkillCraft.Core.Languages
{
  public interface ILanguageQuerier
  {
    Task<Language?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Language>> GetAsync(int worldSid, IEnumerable<Guid> ids, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Language>> GetPagedAsync(int worldSid, bool? isExotic = null, string? search = null,
      LanguageSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
