using AutoMapper;
using SkillCraft.Core.Models;
using SkillCraft.Core.Races;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Mappings
{
  internal class RaceProfile : Profile
  {
    public RaceProfile()
    {
      CreateMap<Race, RaceModel>()
        .IncludeBase<EntityBase, EntityBaseModel>()
        .ForMember(x => x.AgeThresholds, x => x.MapFrom(y => y.AgeThresholds == null ? null : new AgeThresholdsModel
        {
          Teenager = y.AgeThresholds[0],
          Adult = y.AgeThresholds[1],
          Mature = y.AgeThresholds[2],
          Venerable = y.AgeThresholds[3]
        }))
        .ForMember(x => x.Attributes, x => x.MapFrom(y => y.Attributes.Select(z => new AttributeBonusModel
        {
          Attribute = z.Key,
          Bonus = z.Value
        })))
        .ForMember(x => x.Names, x => x.MapFrom(y => y.Names.Select(z => new NameCategoryModel
        {
          Category = z.Key,
          Values = z.Value
        })))
        .ForMember(x => x.Speeds, x => x.MapFrom(y => y.Speeds.Select(z => new RacialSpeedModel
        {
          Type = z.Key,
          Value = z.Value
        })))
        .ForMember(x => x.WeightRolls, x => x.MapFrom(y => y.WeightRolls == null ? null : new WeightRollsModel()
        {
          Skinny = y.WeightRolls[0],
          Thin = y.WeightRolls[1],
          Normal = y.WeightRolls[2],
          Overweight = y.WeightRolls[3],
          Obese = y.WeightRolls[4]
        }));
      CreateMap<RacialTrait, RacialTraitModel>();
    }
  }
}
