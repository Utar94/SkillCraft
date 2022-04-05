using AutoMapper;
using SkillCraft.Core.Worlds;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Mapping
{
  internal class WorldProfile : Profile
  {
    public WorldProfile()
    {
      CreateMap<World, WorldModel>().MapAggregate();
    }
  }
}
