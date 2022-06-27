using SkillCraft.Core.Models;

namespace SkillCraft.Core.Powers.Models
{
  public class PowerModel : EntityBaseModel
  {
    public int Tier { get; set; }

    public string Name { get; set; } = null!;
    public DescriptionsModel Descriptions { get; set; } = null!;

    public IncantationType Incantation { get; set; }
    public int? Duration { get; set; }
    public int? Range { get; set; }
    public string? Ingredients { get; set; }

    public bool Concentration { get; set; }
    public bool Focus { get; set; }
    public bool Ritual { get; set; }
    public bool Somatic { get; set; }
    public bool Verbal { get; set; }
  }
}
