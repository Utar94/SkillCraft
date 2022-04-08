using AutoMapper;
using SkillCraft.Core.Talents;
using SkillCraft.Core.Talents.Models;

namespace SkillCraft.Core.Mapping
{
  internal class TalentProfile : Profile
  {
    public TalentProfile()
    {
      CreateMap<Talent, TalentModel>().MapAggregate()
        .ForMember(x => x.RequiredTalentId, x => x.MapFrom(GetRequiredTalentId));
    }

    private static Guid? GetRequiredTalentId(Talent talent, TalentModel model)
    {
      if (talent.RequiredTalentId.HasValue)
      {
        return talent.RequiredTalent?.Key ?? throw new ArgumentException($"The {nameof(talent.RequiredTalent)} is required.", nameof(talent));
      }

      return null;
    }
  }
}
