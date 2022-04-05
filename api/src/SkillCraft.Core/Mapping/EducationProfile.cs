using AutoMapper;
using SkillCraft.Core.Educations;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Mapping
{
  internal class EducationProfile : Profile
  {
    public EducationProfile()
    {
      CreateMap<Education, EducationModel>().MapAggregate();
    }
  }
}
