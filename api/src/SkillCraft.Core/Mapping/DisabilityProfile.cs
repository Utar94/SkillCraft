using AutoMapper;
using SkillCraft.Core.Disabilities;
using SkillCraft.Core.Disabilities.Models;

namespace SkillCraft.Core.Mapping
{
  internal class DisabilityProfile : Profile
  {
    public DisabilityProfile()
    {
      CreateMap<Disability, DisabilityModel>().MapAggregate();
    }
  }
}
