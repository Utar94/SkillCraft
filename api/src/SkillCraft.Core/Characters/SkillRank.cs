namespace SkillCraft.Core.Characters
{
  public class SkillRank
  {
    public SkillRank(Skill skill, Guid userId)
    {
      CreatedAt = DateTime.UtcNow;
      CreatedById = userId;
      Id = Guid.NewGuid();
      Skill = skill;
    }
    private SkillRank()
    {
    }

    public Skill Skill { get; set; }
    public bool Training { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedById { get; set; }
    public Guid Id { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedById { get; set; }
    public int Version { get; set; }

    public void Delete(Guid userId)
    {
      Deleted = true;
      DeletedAt = DateTime.UtcNow;
      DeletedById = userId;
    }

    public void Update(Guid userId)
    {
      UpdatedAt = DateTime.UtcNow;
      UpdatedById = userId;
      Version++;
    }
  }
}
