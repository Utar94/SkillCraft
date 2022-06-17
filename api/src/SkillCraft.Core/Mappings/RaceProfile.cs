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
        })));
      CreateMap<RacialTrait, RacialTraitModel>();
    }
  }
}
