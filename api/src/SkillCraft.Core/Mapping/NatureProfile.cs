using AutoMapper;
using SkillCraft.Core.Natures;
using SkillCraft.Core.Natures.Models;

namespace SkillCraft.Core.Mapping
{
  internal class NatureProfile : Profile
  {
    public NatureProfile()
    {
      CreateMap<Nature, NatureModel>().MapAggregate()
        .ForMember(x => x.GiftId, x => x.MapFrom(GetGiftId));
    }

    private static Guid? GetGiftId(Nature nature, NatureModel model)
    {
      if (nature.GiftId.HasValue)
      {
        return nature.Gift?.Key ?? throw new ArgumentException($"The {nameof(nature.Gift)} is required.", nameof(nature));
      }

      return null;
    }
  }
}
