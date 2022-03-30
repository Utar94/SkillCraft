namespace SkillCraft.Core.Models
{
  public abstract class AggregateModel
  {
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid Id { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
  }
}
