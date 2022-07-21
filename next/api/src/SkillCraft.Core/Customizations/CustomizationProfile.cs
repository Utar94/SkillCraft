using AutoMapper;
using SkillCraft.Core.Customizations.Models;

namespace SkillCraft.Core.Customizations
{
  internal class CustomizationProfile : Profile
  {
    public CustomizationProfile()
    {
      CreateMap<Customization, CustomizationModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
