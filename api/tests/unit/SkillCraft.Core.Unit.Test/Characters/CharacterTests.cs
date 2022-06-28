using SkillCraft.Core.Classes;
using SkillCraft.Core.Fakers;
using SkillCraft.Core.Powers;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Characters
{
  [Trait(Traits.Category, Categories.Unit)]
  public class CharacterTests
  {
    private static readonly WorldFaker _worldFaker = new();

    private readonly ExperienceTable _experienceTable = new();
    private readonly World _world = _worldFaker.Generate();
    private readonly Character _character;

    public CharacterTests()
    {
      _character = new(UserId, _world);
    }

    private Guid UserId => _world.CreatedById;

    [Fact]
    public void Given_Character_When_getMaxStamina_Then_CorrectMaxStamina()
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Vigor] = 10;
      _character.Creation.BestAttribute = Attribute.Vigor;

      _character.Experience = 11908;
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      _character.Bonuses.Add(new OtherBonus(OtherBonusTarget.Stamina)
      {
        Description = "Endurance I",
        Permanent = true,
        Value = 5
      });
      _character.Bonuses.Add(new OtherBonus(OtherBonusTarget.Stamina)
      {
        Description = "Endurance II",
        Permanent = true,
        Value = 10
      });

      Assert.Equal(92, _character.MaxStamina);
    }

    [Fact]
    public void Given_Character_When_getMaxVitality_Then_CorrectMaxVitality()
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Vigor] = 10;
      _character.Creation.BestAttribute = Attribute.Vigor;

      _character.Experience = 11908;
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      Assert.Equal(77, _character.MaxVitality);
    }

    [Fact]
    public void Given_Classes_When_GetTier_Then_CorrectTier()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(0, UserId, _world)
      {
        Class = new Class(1, UserId, _world)
      }));
      _character.Talents.Add(new CharacterTalent(_character, new Talent(1, UserId, _world)
      {
        Class = new Class(2, UserId, _world)
      }));
      _character.Talents.Add(new CharacterTalent(_character, new Talent(0, UserId, _world)
      {
        Class = new Class(1, UserId, _world)
      }));

      Assert.Equal(2, _character.Tier);
    }

    [Fact]
    public void Given_ExceededSkillRanks_When_Validate_Then_SkillRanksExceededException()
    {
      _character.Creation ??= new();
      _character.Creation.AttributeBases[Attribute.Intellect] = 11;
      _character.Creation.BestAttribute = Attribute.Intellect;
      _character.Creation.MandatoryAttribute1 = Attribute.Intellect;

      _character.SkillRanks.Add(new SkillRank(Skill.Discipline, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Discipline, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Discipline, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Sorcery, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Sorcery, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Sorcery, true));

      var exception = Assert.Throws<SkillRanksExceededException>(() => _character.Validate());
      Assert.Equal(new[] { Skill.Discipline, Skill.Sorcery }, exception.Skills);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(123)]
    [InlineData(1099)]
    [InlineData(1100)]
    [InlineData(268000)]
    [InlineData(999999)]
    public void Given_Experience_When_getLevel_Then_CorrectLevel(int experience)
    {
      int expected = _experienceTable.GetLevel(experience);

      _character.Experience = experience;

      Assert.Equal(expected, _character.Level);
    }

    [Theory]
    [InlineData(9)]
    public void Given_Level_When_getTotalLearningPoints_Then_CorrectTotalLearningPoints(int level)
    {
      _character.Creation = new();
      _character.Creation.AttributeBases[Attribute.Intellect] = 10;
      _character.Experience = _experienceTable.GetThreshold(level);

      for (int current = 0; current < level; current++)
      {
        _character.LevelUp(Attribute.Mind);
      }

      Assert.Equal(5 + 2 * level, _character.TotalLearningPoints);
    }

    [Theory]
    [InlineData(9)]
    public void Given_Level_When_getTotalTalentPoints_Then_CorrectTotalTalentPoints(int level)
    {
      _character.Experience = _experienceTable.GetThreshold(level);

      int expected = (level + 1) * 4;

      Assert.Equal(expected, _character.TotalTalentPoints);
    }

    [Theory]
    [InlineData(Attribute.Vigor)]
    public void Given_LevelUps_When_LevelUp_Then_LeveledUp(Attribute attribute)
    {
      _character.Experience = 11908;

      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);

      CharacterLevelUp levelUp = _character.LevelUp(attribute);
      Assert.Same(_character.LevelUps[_character.Level], levelUp);
    }

    [Theory]
    [InlineData(Attribute.Vigor)]
    public void Given_MaxLevel_When_LevelUp_Then_CharacterCannotLevelUpException(Attribute attribute)
    {
      _character.Experience = 11908;

      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Presence);
      _character.LevelUp(Attribute.Coordination);
      _character.LevelUp(Attribute.Vigor);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(Attribute.Mind);
      _character.LevelUp(attribute);

      var exception = Assert.Throws<CharacterCannotLevelUpException>(() => _character.LevelUp(attribute));
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_MissingRequiredTalents_When_Validate_Then_MissingRequiredTalentsException()
    {
      var requiredTalent1 = new Talent(0, UserId, _world)
      {
        Id = 1
      };
      var requiredTalent2 = new Talent(0, UserId, _world)
      {
        Id = 2
      };

      var talent1 = new Talent(0, UserId, _world)
      {
        RequiredTalent = requiredTalent1,
        RequiredTalentId = requiredTalent1.Id
      };
      var talent2 = new Talent(0, UserId, _world)
      {
        RequiredTalent = requiredTalent2,
        RequiredTalentId = requiredTalent2.Id
      };
      var talent3 = new Talent(0, UserId, _world)
      {
        RequiredTalent = requiredTalent2,
        RequiredTalentId = requiredTalent2.Id
      };

      _character.Talents.Add(new CharacterTalent(_character, talent1));
      _character.Talents.Add(new CharacterTalent(_character, talent2));
      _character.Talents.Add(new CharacterTalent(_character, talent3));

      var exception = Assert.Throws<MissingRequiredTalentsException>(() => _character.Validate());
      Assert.Equal(new[] { requiredTalent1.Id, requiredTalent2.Id }, exception.RequiredTalents.Select(x => x.Id).Distinct());
    }

    [Theory]
    [InlineData(Attribute.Sensitivity)]
    public void Given_NoLevelUp_When_LevelUp_Then_LeveledUp(Attribute attribute)
    {
      _character.Experience = _experienceTable.GetThreshold(1);

      CharacterLevelUp levelUp = _character.LevelUp(attribute);
      Assert.Same(_character.LevelUps.Values.Single(), levelUp);
    }

    [Fact]
    public void Given_NoClass_When_getTier_Then_Zero()
    {
      Assert.Equal(0, _character.Tier);
    }

    [Fact]
    public void Given_NoUniqueTalent_When_getClasses_Then_Empty()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(0, UserId, _world)));

      Assert.Empty(_character.Classes);
    }

    [Fact]
    public void Given_SkillRanks_When_Validate_Then_Success()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Stealth, training: true));

      _character.Validate();
    }

    [Fact]
    public void Given_SkillRanks_When_getRemainingLearningPoints_Then_CorrectRemainingLearningPoints()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Craft, training: false));
      _character.SkillRanks.Add(new SkillRank(Skill.Orientation, training: true));

      Assert.Equal(2, _character.RemainingLearningPoints);
    }

    [Fact]
    public void Given_SkillRanks_When_getSpentLearningPoints_Then_CorrectSpentLearningPoints()
    {
      _character.SkillRanks.Add(new SkillRank(Skill.Acrobatics, false));
      _character.SkillRanks.Add(new SkillRank(Skill.Insight, true));
      _character.SkillRanks.Add(new SkillRank(Skill.Sorcery, true));

      int expected = _character.SkillRanks.Sum(x => x.Cost);

      Assert.Equal(expected, _character.SpentLearningPoints);
    }

    [Fact]
    public void Given_SpentLearningPointsExceeded_When_Validate_Then_SpentLearningPointsExceededException()
    {
      _character.SkillRanks.Add(new(Skill.Insight, training: false));
      _character.SkillRanks.Add(new(Skill.Insight, training: false));
      _character.SkillRanks.Add(new(Skill.Investigation, training: false));

      var exception = Assert.Throws<SpentLearningPointsExceededException>(() => _character.Validate());
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_SpentTalentPointsExceeded_When_Validate_Then_SpentTalentPointsExceededException()
    {
      _character.Powers.Add(new CharacterPower(_character, new Power(tier: 2, UserId, _world))
      {
        Cost = 2
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 3, UserId, _world))
      {
        Cost = 3
      });

      var exception = Assert.Throws<SpentTalentPointsExceededException>(() => _character.Validate());
      Assert.Same(_character, exception.Character);
    }

    [Fact]
    public void Given_TalentsAndPowers_When_getRemainingTalentPoints_Then_CorrectRemainingTalentPoints()
    {
      _character.Powers.Add(new CharacterPower(_character, new Power(tier: 1, UserId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 2
      });

      Assert.Equal(1, _character.RemainingTalentPoints);
    }

    [Fact]
    public void Given_TalentsAndPowers_When_getSpentTalentPoints_Then_CorrectSpentTalentPoints()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 1
      });
      _character.Talents.Add(new CharacterTalent(_character, new Talent(tier: 0, UserId, _world))
      {
        Cost = 2
      });
      _character.Powers.Add(new CharacterPower(_character, new Power(tier: 1, UserId, _world))
      {
        Cost = 3
      });

      Assert.Equal(6, _character.SpentTalentPoints);
    }

    [Fact]
    public void Given_TalentsAndPowers_When_Validate_Then_Success()
    {
      var requiredTalent = new Talent(tier: 0, UserId, _world)
      {
        Id = 1
      };
      var requiringTalent = new Talent(tier: 0, UserId, _world)
      {
        RequiredTalent = requiredTalent,
        RequiredTalentId = requiredTalent.Id
      };

      _character.Talents.Add(new CharacterTalent(_character, requiredTalent)
      {
        Cost = 0
      });
      _character.Talents.Add(new CharacterTalent(_character, requiringTalent)
      {
        Cost = 1
      });
      _character.Powers.Add(new CharacterPower(_character, new Power(tier: 0, UserId, _world))
      {
        Cost = 2
      });

      _character.Validate();
    }

    [Fact]
    public void Given_Talents_When_getClasses_Then_CorrectClasses()
    {
      _character.Talents.Add(new CharacterTalent(_character, new Talent(0, UserId, _world)));

      var class1 = new Class(1, UserId, _world)
      {
        Id = 1
      };
      _character.Talents.Add(new CharacterTalent(_character, new Talent(0, UserId, _world)
      {
        Class = class1
      }));

      var class2 = new Class(2, UserId, _world)
      {
        Id = 2
      };
      _character.Talents.Add(new CharacterTalent(_character, new Talent(1, UserId, _world)
      {
        Class = class2
      }));

      Assert.Equal(new[] { class1, class2 }, _character.Classes);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 5)]
    [InlineData(2, 9)]
    [InlineData(3, 14)]
    public void Given_Tier_When_getMaxSkillRank_Then_CorrectMaxSkillRank(int tier, int maxSkillRank)
    {
      Assert.True(tier >= 0 && tier <= 3);

      if (tier > 0)
      {
        _character.Talents.Add(new CharacterTalent(_character, new Talent(tier - 1, UserId, _world)
        {
          Class = new Class(tier, UserId, _world)
        }));
        Assert.Equal(tier, _character.Tier);
      }

      Assert.Equal(maxSkillRank, _character.MaxSkillRank);
    }
  }
}
