namespace SkillCraft.Core.Educations
{
  public interface IEducationQuerier
  {
    Task<Education?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Education>> GetPagedAsync(int worldSid, string? search = null, Skill? skill = null,
      EducationSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
