using AutoMapper;
using SkillCraft.Core.Powers.Models;

namespace SkillCraft.Core.Powers
{
  internal class PowerProfile : Profile
  {
    public PowerProfile()
    {
      CreateMap<Power, PowerModel>()
        .IncludeBase<Aggregate, AggregateModel>()
        .ForMember(x => x.Descriptions, x => x.MapFrom(GetDescriptions));
    }

    private static DescriptionsModel? GetDescriptions(Power power, PowerModel model)
    {
      if (power.Descriptions == null)
      {
        return null;
      }
      else if (power.Descriptions.Length == 3)
      {
        return new()
        {
          FirstLevel = power.Descriptions[0],
          SecondLevel = power.Descriptions[1],
          ThirdLevel = power.Descriptions[2]
        };
      }
      else if (power.Descriptions.Length == 4)
      {
        return new()
        {
          Global = power.Descriptions[0],
          FirstLevel = power.Descriptions[1],
          SecondLevel = power.Descriptions[2],
          ThirdLevel = power.Descriptions[3]
        };
      }
      else
      {
        throw new ArgumentException($"The {nameof(power.Descriptions)} may only contain 3 or 4 elements.", nameof(power));
      }
    }
  }
}
