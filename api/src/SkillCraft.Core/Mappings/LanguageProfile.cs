using AutoMapper;
using SkillCraft.Core.Languages;
using SkillCraft.Core.Languages.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class LanguageProfile : Profile
  {
    public LanguageProfile()
    {
      CreateMap<Language, LanguageModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
    }
  }
}
