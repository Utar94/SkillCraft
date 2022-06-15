using AutoMapper;
using SkillCraft.Core.Castes;
using SkillCraft.Core.Castes.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class CasteProfile : Profile
  {
    public CasteProfile()
    {
      CreateMap<Caste, CasteModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
