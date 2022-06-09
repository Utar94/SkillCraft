using AutoMapper;
using SkillCraft.Core.Models;
using SkillCraft.Core.Worlds;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Mappings
{
  internal class WorldProfile : Profile
  {
    public WorldProfile()
    {
      CreateMap<World, WorldModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
