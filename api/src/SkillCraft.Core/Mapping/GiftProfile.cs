using AutoMapper;
using SkillCraft.Core.Gifts;
using SkillCraft.Core.Gifts.Models;

namespace SkillCraft.Core.Mapping
{
  internal class GiftProfile : Profile
  {
    public GiftProfile()
    {
      CreateMap<Gift, GiftModel>().MapAggregate();
    }
  }
}
