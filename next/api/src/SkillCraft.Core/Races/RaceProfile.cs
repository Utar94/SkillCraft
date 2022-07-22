using AutoMapper;
using SkillCraft.Core.Races.Models;

namespace SkillCraft.Core.Races
{
  internal class RaceProfile : Profile
  {
    public RaceProfile()
    {
      CreateMap<Race, RaceModel>()
        .IncludeBase<Aggregate, AggregateModel>()
        .ForMember(x => x.Attributes, x => x.MapFrom(y => new AttributeBonusesModel
        {
          Agility = y.Attributes.ContainsKey(Attribute.Agility) ? y.Attributes[Attribute.Agility] : 0,
          Coordination = y.Attributes.ContainsKey(Attribute.Coordination) ? y.Attributes[Attribute.Coordination] : 0,
          Intellect = y.Attributes.ContainsKey(Attribute.Intellect) ? y.Attributes[Attribute.Intellect] : 0,
          Mind = y.Attributes.ContainsKey(Attribute.Mind) ? y.Attributes[Attribute.Mind] : 0,
          Presence = y.Attributes.ContainsKey(Attribute.Presence) ? y.Attributes[Attribute.Presence] : 0,
          Sensitivity = y.Attributes.ContainsKey(Attribute.Agility) ? y.Attributes[Attribute.Sensitivity] : 0,
          Vigor = y.Attributes.ContainsKey(Attribute.Vigor) ? y.Attributes[Attribute.Vigor] : 0,
        }))
        .ForMember(x => x.Names, x => x.MapFrom(y => y.Names.Select(z => new NameCategoryModel
        {
          Category = z.Key,
          Values = z.Value
        })))
        .ForMember(x => x.Speeds, x => x.MapFrom(y => new RacialSpeedModel
        {
          Burrow = y.Speeds.ContainsKey(SpeedType.Burrow) ? y.Speeds[SpeedType.Burrow] : 0,
          Climb = y.Speeds.ContainsKey(SpeedType.Climb) ? y.Speeds[SpeedType.Climb] : 0,
          Fly = y.Speeds.ContainsKey(SpeedType.Fly) ? y.Speeds[SpeedType.Fly] : 0,
          Swim = y.Speeds.ContainsKey(SpeedType.Burrow) ? y.Speeds[SpeedType.Swim] : 0,
          Walk = y.Speeds.ContainsKey(SpeedType.Walk) ? y.Speeds[SpeedType.Walk] : 0,
        }))
        .ForMember(x => x.AgeThresholds, x => x.MapFrom(y => y.AgeThresholds == null ? null : new AgeThresholdsModel
        {
          Teenager = y.AgeThresholds[0],
          Adult = y.AgeThresholds[1],
          Mature = y.AgeThresholds[2],
          Venerable = y.AgeThresholds[3]
        }))
        .ForMember(x => x.WeightRolls, x => x.MapFrom(y => y.WeightRolls == null ? null : new WeightRollsModel
        {
          Skinny = y.WeightRolls[0],
          Thin = y.WeightRolls[1],
          Normal = y.WeightRolls[2],
          Overweight = y.WeightRolls[4],
          Obese = y.WeightRolls[3]
        }));
      CreateMap<RacialTrait, RacialTraitModel>();
    }
  }
}
