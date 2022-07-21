using AutoMapper;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Natures
{
  internal class NatureProfile : Profile
  {
    public NatureProfile()
    {
      CreateMap<Nature, NatureModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
