namespace SkillCraft.Infrastructure
{
  public interface IDatabaseService
  {
    Task InitializeAsync(CancellationToken cancellationToken = default);
  }
}
