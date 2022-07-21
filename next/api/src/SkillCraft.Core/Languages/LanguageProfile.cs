using AutoMapper;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Languages
{
  internal class LanguageProfile : Profile
  {
    public LanguageProfile()
    {
      CreateMap<Language, LanguageModel>()
        .IncludeBase<Aggregate, AggregateModel>();
    }
  }
}
