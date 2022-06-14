namespace SkillCraft.Core.Models
{
  public abstract class EntityBaseModel
  {
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid Id { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
  }
}
