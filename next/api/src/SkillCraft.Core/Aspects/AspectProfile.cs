using AutoMapper;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Aspects
{
  internal class AspectProfile : Profile
  {
    public AspectProfile()
    {
      CreateMap<Aspect, AspectModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
