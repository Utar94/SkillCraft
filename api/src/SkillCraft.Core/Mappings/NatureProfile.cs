using AutoMapper;
using SkillCraft.Core.Models;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Mappings
{
  internal class NatureProfile : Profile
  {
    public NatureProfile()
    {
      CreateMap<Nature, NatureModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
