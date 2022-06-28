using AutoMapper;
using SkillCraft.Core.Classes;
using SkillCraft.Core.Classes.Models;
using SkillCraft.Core.Models;

namespace SkillCraft.Core.Mappings
{
  internal class ClassProfile : Profile
  {
    public ClassProfile()
    {
      CreateMap<Class, ClassModel>()
        .IncludeBase<EntityBase, EntityBaseModel>();
      CreateMap<ClassTalent, ClassTalentModel>();
    }
  }
}
