using AutoMapper;
using SkillCraft.Core.Educations.Models;

namespace SkillCraft.Core.Educations
{
  internal class EducationProfile : Profile
  {
    public EducationProfile()
    {
      CreateMap<Education, EducationModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
