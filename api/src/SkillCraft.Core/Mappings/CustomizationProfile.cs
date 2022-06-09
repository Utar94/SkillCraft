using AutoMapper;
using SkillCraft.Core.Customizations;
using SkillCraft.Core.Customizations.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class CustomizationProfile : Profile
  {
    public CustomizationProfile()
    {
      CreateMap<Customization, CustomizationModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
