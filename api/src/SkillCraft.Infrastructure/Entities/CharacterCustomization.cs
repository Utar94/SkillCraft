using SkillCraft.Core.Characters;
using SkillCraft.Core.Customizations;

namespace SkillCraft.Infrastructure.Entities
{
  internal class CharacterCustomization
  {
    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public int CustomizationId { get; set; }
    public Customization? Customization { get; set; }
  }
}
