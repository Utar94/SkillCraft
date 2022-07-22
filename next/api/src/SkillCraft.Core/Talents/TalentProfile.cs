using AutoMapper;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Talents
{
  internal class TalentProfile : Profile
  {
    public TalentProfile()
    {
      CreateMap<Talent, TalentModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<TalentOption, TalentOptionModel>();
    }
  }
}
