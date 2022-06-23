using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;
using SkillCraft.Core.Natures.Models;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Characters.Models
{
  public class CharacterModel : EntityBaseModel
  {
    public string Name { get; set; } = null!;
    public string? Player { get; set; }

    public AspectModel? Aspect1 { get; set; }
    public AspectModel? Aspect2 { get; set; }
    public CasteModel? Caste { get; set; }
    public EducationModel? Education { get; set; }
    public NatureModel? Nature { get; set; }
    public RaceModel? Race { get; set; }

    public double Stature { get; set; }
    public double Weight { get; set; }
    public int Age { get; set; }

    public int Experience { get; set; }
    public int Vitality { get; set; }
    public int Stamina { get; set; }

    public int BloodAlcoholContent { get; set; }
    public int Intoxication { get; set; }

    public string? Description { get; set; }

    public IEnumerable<BonusModel> Bonuses { get; set; } = null!;
    public CharacterCreationModel? Creation { get; set; }
    public IEnumerable<CharacterLevelUpModel> LevelUps { get; set; } = null!;
    public IEnumerable<SkillRankModel> SkillRanks { get; set; } = null!;

    public IEnumerable<CharacterConditionModel> Conditions { get; set; } = null!;
    public IEnumerable<CustomizationModel> Customizations { get; set; } = null!;
    public IEnumerable<LanguageModel> Languages { get; set; } = null!;
    public IEnumerable<CharacterTalentModel> Talents { get; set; } = null!;
  }
}
