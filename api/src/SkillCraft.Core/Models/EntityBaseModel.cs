namespace SkillCraft.Core.Models
{
  public abstract class EntityBaseModel
  {
    public DateTime CreatedAt { get; set; }
    public Guid Id { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}
