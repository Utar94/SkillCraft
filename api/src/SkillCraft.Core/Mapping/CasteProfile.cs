using AutoMapper;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Castes.Models;

namespace SkillCraft.Core.Mapping
{
  internal class CasteProfile : Profile
  {
    public CasteProfile()
    {
      CreateMap<Caste, CasteModel>().MapAggregate();
      CreateMap<CasteTrait, CasteTraitModel>()
        .ForMember(x => x.Id, x => x.MapFrom(y => y.Key));
    }
  }
}
