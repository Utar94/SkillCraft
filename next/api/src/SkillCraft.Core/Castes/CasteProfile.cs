using AutoMapper;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Castes
{
  internal class CasteProfile : Profile
  {
    public CasteProfile()
    {
      CreateMap<Caste, CasteModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
