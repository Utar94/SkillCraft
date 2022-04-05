using AutoMapper;
using SkillCraft.Core.Worlds;

namespace SkillCraft.Core.Mappings
{
  internal class WorldProfile : Profile
  {
    public WorldProfile()
    {
      CreateMap<World, WorldModel>().MapAggregate();
    }
  }
}
