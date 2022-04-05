using AutoMapper;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Languages.Models;

namespace SkillCraft.Core.Mapping
{
  internal class LanguageProfile : Profile
  {
    public LanguageProfile()
    {
      CreateMap<Language, LanguageModel>().MapAggregate();
    }
  }
}
