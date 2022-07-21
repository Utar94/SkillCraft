using AutoMapper;

namespace SkillCraft.Core
{
  internal class AggregateProfile : Profile
  {
    public AggregateProfile()
    {
      CreateMap<Aggregate, AggregateModel>();
    }
  }
}
