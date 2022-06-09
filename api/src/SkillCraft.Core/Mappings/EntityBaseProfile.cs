using AutoMapper;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class EntityBaseProfile : Profile
  {
    public EntityBaseProfile()
    {
      CreateMap<EntityBase, EntityBaseModel>()
        .ForMember(x => x.Id, x => x.MapFrom(y => y.Uuid));
    }
  }
}
