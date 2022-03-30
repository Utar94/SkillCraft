namespace SkillCraft.Core.Worlds
{
  public class World : Aggregate
  {
    public World(string alias, Guid userId) : base(userId)
    {
      Alias = alias?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(alias));
    }
    private World() : base()
    {
    }

    public string Alias { get; set; } = null!;
    public Confidentiality Confidentiality { get; set; }
    public string? Description { get; set; }
    public string Name { get; set; } = null!;
  }
}
