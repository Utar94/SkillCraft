using AutoMapper;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Educations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class EducationProfile : Profile
  {
    public EducationProfile()
    {
      CreateMap<Education, EducationModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
