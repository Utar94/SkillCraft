namespace SkillCraft.Core.Customizations.Models
{
  public class CustomizationModel : AggregateModel
  {
    public CustomizationType Type { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
  }
}
