using AutoMapper;
using SkillCraft.Core.Models;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Mappings
{
  internal class TalentProfile : Profile
  {
    public TalentProfile()
    {
      CreateMap<Talent, TalentModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
      CreateMap<TalentOption, TalentOptionModel>()
        .ForMember(x => x.Id, x => x.MapFrom(y => y.Uuid));
    }
  }
}
