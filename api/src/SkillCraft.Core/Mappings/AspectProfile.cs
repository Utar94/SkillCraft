using AutoMapper;
using SkillCraft.Core.Aspects;
using SkillCraft.Core.Aspects.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class AspectProfile : Profile
  {
    public AspectProfile()
    {
      CreateMap<Aspect, AspectModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
