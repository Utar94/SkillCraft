namespace SkillCraft.Core.Talents.Models
{
  public class TalentModel : AggregateModel
  {
    public TalentModel? RequiredTalent { get; set; }

    public int Tier { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public bool MultipleAcquisition { get; set; }
    public Skill? Skill { get; set; }

    public IEnumerable<TalentOptionModel> Options { get; set; } = null!;
  }
}
