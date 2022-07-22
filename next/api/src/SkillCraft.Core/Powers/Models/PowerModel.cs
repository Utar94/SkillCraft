namespace SkillCraft.Core.Powers.Models
{
  public class PowerModel : AggregateModel
  {
    public int Tier { get; set; }
    public string Name { get; set; } = null!;
    public DescriptionsModel? Descriptions { get; set; }

    public IncantationType Incantation { get; set; }
    public bool IsRitual { get; set; }

    public bool IsSomatic { get; set; }
    public bool IsVerbal { get; set; }

    public int? Duration { get; set; }
    public bool IsConcentration { get; set; }

    public int? Range { get; set; }

    public string? Ingredients { get; set; }
    public bool IsFocus { get; set; }
  }
}
