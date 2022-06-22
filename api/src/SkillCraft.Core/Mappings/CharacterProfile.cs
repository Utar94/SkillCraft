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
          Constitution = z.Value.Constitution,
          Initiative = z.Value.Initiative,
          Learning = z.Value.Learning,
          Power = z.Value.Power,
          Precision = z.Value.Precision,
          Repute = z.Value.Repute,
          Strength = z.Value.Strength
        })));
      CreateMap<AttributeBases, AttributeBasesModel>();
      CreateMap<CharacterCondition, CharacterConditionModel>();
      CreateMap<CharacterCreation, CharacterCreationModel>();
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
  }
}
