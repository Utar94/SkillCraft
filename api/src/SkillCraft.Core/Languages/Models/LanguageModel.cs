using SkillCraft.Core.Models;

namespace SkillCraft.Core.Languages.Models
{
  public class LanguageModel : EntityBaseModel
  {
    public string? Description { get; set; }
    public string Name { get; set; } = null!;

    public bool Exotic { get; set; }
    public string? Script { get; set; }
    public string? TypicalSpeakers { get; set; }
  }
}
