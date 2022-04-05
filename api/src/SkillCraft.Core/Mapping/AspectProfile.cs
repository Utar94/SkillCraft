using AutoMapper;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Aspects.Models;

namespace SkillCraft.Core.Mapping
{
  internal class AspectProfile : Profile
  {
    public AspectProfile()
    {
      CreateMap<Aspect, AspectModel>().MapAggregate();
    }
  }
}
