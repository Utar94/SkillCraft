using AutoMapper;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class CharacterProfile : Profile
  {
    public CharacterProfile()
    {
      CreateMap<Character, CharacterModel>()
        .IncludeBase<EntityBase, EntityBaseModel>()
        .ForMember(x => x.LevelUps, x => x.MapFrom(y => y.LevelUps.Select(z => new CharacterLevelUpModel()
        {
          Level = z.Key,
          Attribute = z.Value.Attribute,
          Constitution = (int)GetStatisticIncrement(Statistic.Constitution, z.Value),
          Initiative = GetStatisticIncrement(Statistic.Initiative, z.Value),
          Learning = (int)GetStatisticIncrement(Statistic.Learning, z.Value),
          Power = GetStatisticIncrement(Statistic.Power, z.Value),
          Precision = GetStatisticIncrement(Statistic.Precision, z.Value),
          Repute = GetStatisticIncrement(Statistic.Repute, z.Value),
          Strength = GetStatisticIncrement(Statistic.Strength, z.Value)
        })));
      CreateMap<CharacterCondition, CharacterConditionModel>();
      CreateMap<CharacterCreation, CharacterCreationModel>()
        .ForMember(x => x.AttributeBases, x => x.MapFrom(y => new AttributeBasesModel
        {
          Agility = GetAttributeBase(Attribute.Agility, y),
          Coordination = GetAttributeBase(Attribute.Coordination, y),
          Intellect = GetAttributeBase(Attribute.Intellect, y),
          Mind = GetAttributeBase(Attribute.Mind, y),
          Presence = GetAttributeBase(Attribute.Presence, y),
          Sensitivity = GetAttributeBase(Attribute.Sensitivity, y),
          Vigor = GetAttributeBase(Attribute.Vigor, y)
        }));
      CreateMap<CharacterPower, CharacterPowerModel>()
        .ForMember(x => x.Id, x => x.MapFrom(y => y.Uuid));
      CreateMap<CharacterTalent, CharacterTalentModel>()
        .ForMember(x => x.Id, x => x.MapFrom(y => y.Uuid));
      CreateMap<SkillRank, SkillRankModel>();

      CreateMap<BonusBase, BonusModel>();
      CreateMap<AttributeBonus, BonusModel>()
        .ForMember(x => x.Type, x => x.MapFrom(y => BonusType.Attribute))
        .ForMember(x => x.Target, x => x.MapFrom(y => y.Attribute));
      CreateMap<AttributeBonus, BonusModel>()
        .ForMember(x => x.Type, x => x.MapFrom(y => BonusType.Attribute))
        .ForMember(x => x.Target, x => x.MapFrom(y => y.Attribute));
      CreateMap<OtherBonus, BonusModel>()
        .ForMember(x => x.Type, x => x.MapFrom(y => BonusType.Other))
        .ForMember(x => x.Target, x => x.MapFrom(y => y.Target));
      CreateMap<StatisticBonus, BonusModel>()
        .ForMember(x => x.Type, x => x.MapFrom(y => BonusType.Statistic))
        .ForMember(x => x.Target, x => x.MapFrom(y => y.Statistic));
    }

    private static int GetAttributeBase(Attribute attribute, CharacterCreation creation)
    {
      return creation.AttributeBases.TryGetValue(attribute, out int @base)
        ? @base
        : 0;
    }
    private static double GetStatisticIncrement(Statistic statistic, CharacterLevelUp levelUp)
    {
      return levelUp.Statistics.TryGetValue(statistic, out double increment)
        ? increment
        : 0.0;
    }
  }
}
