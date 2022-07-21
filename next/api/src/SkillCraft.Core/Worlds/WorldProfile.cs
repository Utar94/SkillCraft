using AutoMapper;
using SkillCraft.Core.Worlds.Models;

namespace SkillCraft.Core.Worlds
{
  internal class WorldProfile : Profile
  {
    public WorldProfile()
    {
      CreateMap<World, WorldModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
