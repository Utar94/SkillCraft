namespace SkillCraft.Core.Talents
{
  public interface ITalentQuerier
  {
    Task<Talent?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Talent>> GetPagedAsync(int worldSid, bool? multipleAcquisition = null, string? search = null, Skill? skill = null, IEnumerable<int>? tiers = null,
      TalentSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
