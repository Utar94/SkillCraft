using SkillCraft.Core.Models;

namespace SkillCraft.Core.Languages.Models
{
  public class LanguageModel : AggregateModel
  {
    public string? Description { get; set; }
    public bool Exotic { get; set; }
    public string Name { get; set; } = null!;
    public string? Script { get; set; }
    public string? TypicalSpeakers { get; set; }
  }
}
